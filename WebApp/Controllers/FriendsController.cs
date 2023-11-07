using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;
using WebApp.Stores;

namespace WebApp.Controllers
{
    public class FriendsController : Controller
    {
        private readonly SessionService _session;

        public FriendsController(SessionService session)
        {
            _session = session;
        }

        // GET: Friends
        public ActionResult Index()
        {
            return View();
        }

        // GET: Friends/List
        public ActionResult List()
        {
            var user = _session.CurrentUser;
            if (user == null)
                return RedirectToAction("Index", "Home");
            return Json(user.Friends);
        }

        // POST: Friends/Add/{login}
        [HttpPost]
        public ActionResult Add(string login)
        {
            var user = _session.CurrentUser;

            if (user == null || string.IsNullOrEmpty(login.Trim()))
                return Json(false);

            var friend = UserStore.Instance.GetUserByLogin(login);
            if (friend == null)
                return Json(false);

            if (user.Friends.Contains(friend) || friend == user)
                return Json(false);

            user.Friends.Add(friend);
            return Json(true);
        }

        // POST: Friends/Del/{login}
        [HttpDelete]
        public ActionResult Del(string login)
        {
            return Json(false);
        }
    }
}
