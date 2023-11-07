using WebApp.Models;
using WebApp.Stores;

namespace WebApp.Services;

public class SessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public User? CurrentUser
    {
        get
        {
            var login = _httpContextAccessor.HttpContext!.Session.GetString("login");
            if (login == null) return null;
            var user = UserStore.Instance.GetUserByLogin(login);
            return user;
        }
        set
        {
            _httpContextAccessor.HttpContext!.Session.SetString("login", value == null ? "" : value.Login);
        }
    }

    public bool IsLoggedIn => CurrentUser != null;

    public bool IsAdmin => CurrentUser != null && "admin".Equals(CurrentUser.Login);
}
