using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;
using WebApp.Stores;

namespace WebApp.Controllers;

public class UserController : Controller
{
    private readonly SessionService _session;

    public UserController(SessionService session)
    {
        _session = session;
    }

    // GET: User/List
    public IActionResult List()
    {
        if(!_session.IsAdmin)
            return RedirectToAction("Index", "Home");
        return View(UserStore.Instance.Users);
    }

    // GET: User/Add
    public IActionResult Add()
    {
        if (!_session.IsAdmin)
            return RedirectToAction("Index", "Home");
        return View();
    }

    // POST: User/Add
    [HttpPost]
    public IActionResult Add(string login)
    {
        if (!_session.IsAdmin)
            return RedirectToAction("Index", "Home");

        if (string.IsNullOrEmpty(login))
        {
            ViewData["error"] = "Incorrect login";
            return View();
        }
        if(UserStore.Instance.HasUser(login))
        {
            ViewData["error"] = "User already exists";
            return View();
        }
        var newUser = new User(login, DateTime.Now);
        UserStore.Instance.AddUser(newUser);
        return RedirectToAction("List");
    }

    // GET: User/Del/{login}
    public IActionResult Del(string login)
    {
        if (!_session.IsAdmin)
            return RedirectToAction("Index", "Home");

        UserStore.Instance.RemoveUserByLogin(login);
        return RedirectToAction("List");
    }

}
