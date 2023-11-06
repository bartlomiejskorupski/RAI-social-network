using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

public class UserController : Controller
{
    private readonly UserSession _session;

    public UserController(UserSession session)
    {
        _session = session;
    }

    // GET: User/List
    public IActionResult List()
    {
        if(_session.CurrentUser == null || _session.CurrentUser.Login != "admin")
            return RedirectToAction("Index", "Home");
        return View(UserStore.Instance.Users);
    }

    // GET: User/Add
    public IActionResult Add()
    {
        return View();
    }

    // POST: User/Add
    [HttpPost]
    public IActionResult Add(string login)
    {
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
        UserStore.Instance.RemoveUserByLogin(login);
        return RedirectToAction("List");
    }

    // GET: User/Login
    public IActionResult Login() 
    {
        return View();
    }

    // POST: User/Login/{login}
    [HttpPost]
    public IActionResult Login(string login)
    {
        if (!UserStore.Instance.HasUser(login))
            return RedirectToAction("Login");
        _session.CurrentUser = UserStore.Instance.GetUserByLogin(login);
        return RedirectToAction("Index", "Home");
    }

    // POST: User/Logout
    [HttpPost]
    public IActionResult Logout()
    {
        _session.CurrentUser = null;
        return RedirectToAction("Index", "Home");
    }

}
