using Npgsql;
using Microsoft.Extensions.Configuration;
using CursinhoEACH.Data;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddTransient<NpgsqlConnection>(_ => new NpgsqlConnection(connString));

SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
builder.Services.AddScoped<DbInitializer>();
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<ClassService>();
builder.Services.AddScoped<MockService>();
builder.Services.AddScoped<EvasionService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<EvasionNotificationService>();

builder.Services.AddHostedService<WeeklyJobService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var initializer = services.GetRequiredService<DbInitializer>();
        await initializer.InitializeAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao criar as tabelas no banco de dados.");
    }
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
