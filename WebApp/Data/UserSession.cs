using WebApp.Models;

namespace WebApp.Data;

public class UserSession
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserSession(IHttpContextAccessor httpContextAccessor)
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
}
