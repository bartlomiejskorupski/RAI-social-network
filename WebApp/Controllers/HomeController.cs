﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SessionService _session;

    public HomeController(
        ILogger<HomeController> logger,
        SessionService session
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