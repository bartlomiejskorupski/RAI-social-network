using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserSession _session;

    public HomeController(
        ILogger<HomeController> logger,
        UserSession session
    ) {
        _logger = logger;
        _session = session;
    }

    public IActionResult Index()
    {
        if(_session.CurrentUser != null)
            ViewBag.Login = _session.CurrentUser.Login;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}