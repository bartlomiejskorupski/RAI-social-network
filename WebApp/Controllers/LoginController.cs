using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using WebApp.Stores;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly SessionService _session;

        public LoginController(SessionService session)
        {
            _session = session;
        }

        // POST: Login/{login}
        [HttpPost]
        public IActionResult Index(string login)
        {
            if(_session.IsLoggedIn)
                return RedirectToAction("Index", "Home");

            if (!UserStore.Instance.HasUser(login))
                return RedirectToAction("Index", "Home");

            _session.CurrentUser = UserStore.Instance.GetUserByLogin(login);
            return RedirectToAction("Index", "Friends");
        }

        // POST: Logout
        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            _session.CurrentUser = null;
            return RedirectToAction("Index", "Home");
        }
    }
}
