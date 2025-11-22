using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CursinhoEACH.Models;

namespace CursinhoEACH.Controllers;

public class ClassController : Controller
{
    private readonly ILogger<ClassController> _logger;

    public ClassController(ILogger<ClassController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index() => View();
    public IActionResult Create() => View();
    [ValidateAntiForgeryToken]
    public IActionResult CreatePost()
    {
        var anoStr = (Request.Form["Ano"].ToString() ?? string.Empty).Trim();
        var periodoStr = (Request.Form["Periodo"].ToString() ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(anoStr) || string.IsNullOrWhiteSpace(periodoStr))
            return RedirectToAction("Index");

        // Expect ano as 4 digits and período code as M/V/N
        var code = periodoStr.Length > 0 ? char.ToUpperInvariant(periodoStr[0]) : ' ';
        if (anoStr.Length != 4 || (code != 'M' && code != 'V' && code != 'N'))
            return RedirectToAction("Index");

        var key = $"{anoStr}{code}"; // e.g., 2025M
        return Redirect($"/Class/{key}");
    }
    [HttpGet("/Class/{key:length(5)}")] public IActionResult Details(string key) => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}