using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
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
            if (user.Friends.Contains(friend.Login) || friend == user)
                return Json(false);

            user.Friends.Add(friend.Login);
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
            if(!user.Friends.Contains(friend.Login))
                return Json(false);

            user.Friends.Remove(friend.Login);
            return Json(true);
        }

        // Get: Friends/Export
        public ActionResult Export()
        {
            if(!_session.IsLoggedIn)
                return RedirectToAction("Index", "Home");

            var user = _session.CurrentUser!;
            var json = JsonConvert.SerializeObject(user.Friends);
            var bytes = Encoding.UTF8.GetBytes(json);
            var filename = $"{user.Login}_friends.json";
            return File(bytes, "application/json", filename);
        }

        // POST: Friends/Import
        [HttpPost]
        public ActionResult Import()
        {
            if(!_session.IsLoggedIn)
                return RedirectToAction("Index", "Home");

            var formFile = Request.Form.Files["importFile"];
            if (formFile == null || !"application/json".Equals(formFile.ContentType))
                return RedirectToAction("Index");

            List<string>? importedList;
            try
            {
                using var streamReader = new StreamReader(formFile.OpenReadStream());
                using var jsonReader = new JsonTextReader(streamReader);
                var serializer = new JsonSerializer();
                importedList = serializer.Deserialize<List<string>>(jsonReader);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

            if (importedList == null)
                return RedirectToAction("Index");

            var invalidUsers = importedList.Where(login => !UserStore.Instance.HasUser(login)).Any();
            if(invalidUsers)
                return RedirectToAction("Index");

            var user = _session.CurrentUser!;
            var distinctFriends = importedList.Distinct()
                .Where(login => login != user.Login);
            user.Friends = distinctFriends.ToList();
            return RedirectToAction("Index");
        }
    }
}
