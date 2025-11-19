using Npgsql;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddTransient<NpgsqlConnection>(_ => new NpgsqlConnection(connString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
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
