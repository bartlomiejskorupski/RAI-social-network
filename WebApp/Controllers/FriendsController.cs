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
            if (!_session.IsLoggedIn)
                return RedirectToAction("Index", "Home");
            return Json(_session.CurrentUser!.Friends);
        }

        // POST: Friends/Add/{login}
        [HttpPost]
        public ActionResult Add(string login)
        {
            if (!_session.IsLoggedIn)
                return Json(false);

            if (string.IsNullOrEmpty(login.Trim()))
                return Json(false);

            var friend = UserStore.Instance.GetUserByLogin(login);
            if (friend == null)
                return Json(false);

            var user = _session.CurrentUser!;
            if (user.Friends.Contains(friend) || friend == user)
                return Json(false);

            user.Friends.Add(friend);
            return Json(true);
        }

        // DELETE: Friends/Del/{login}
        [HttpDelete]
        public ActionResult Del(string login)
        {
            if(!_session.IsLoggedIn) 
                return Json(false);

            var friend = UserStore.Instance.GetUserByLogin(login);
            if (friend == null)
                return Json(false);

            var user = _session.CurrentUser!;
            if(!user.Friends.Contains(friend))
                return Json(false);

            user.Friends.Remove(friend);
            return Json(true);
        }
    }
}
