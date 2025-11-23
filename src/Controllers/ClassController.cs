using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CursinhoEACH.DTO;

namespace CursinhoEACH.Controllers;

public class ClassController : Controller
{
    private readonly ILogger<ClassController> _logger;

    public ClassController(ILogger<ClassController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}