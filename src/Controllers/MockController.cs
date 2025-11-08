using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CursinhoEACH.Models;

namespace CursinhoEACH.Controllers;

public class MockController : Controller
{
    private readonly ILogger<MockController> _logger;

    public MockController(ILogger<MockController> logger)
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