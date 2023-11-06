using WebApp.Models;

namespace WebApp.Data;

public class UserStore
{
    private readonly List<User> _users;
    public List<User> Users => _users;

    private static UserStore? _instance;
    public static UserStore Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UserStore();
            return _instance;
        }
    }
    private UserStore()
    {
        _users = InitUsers();
    }

    public User? GetUserByLogin(string login) 
    {
        return Users.Where(u => u.Login == login).FirstOrDefault();
    }

    public bool HasUser(string login)
    {
        return Users.Where(u => u.Login == login).Any();
    }

    public void AddUser(User user)
    {
        _users.Add(user);
    }

    public void RemoveUserByLogin(string login)
    {
        var user = GetUserByLogin(login);
        if(user != null)
        {
            Users.Remove(user);
        }
    }
    private List<User> InitUsers()
    {
        var users = new List<User>()
        {
            new User("admin", new DateTime(2023, 9, 5, 15, 30, 48)),
            new User("bartek", new DateTime(2023, 10, 7, 17, 33, 23)),
            new User("jan", new DateTime(2023, 10, 13, 11, 42, 27)),
            new User("cakes", new DateTime(2023, 10, 22, 20, 7, 2)),
            new User("marcin", new DateTime(2023, 10, 27, 18, 54, 51)),
            new User("darek", new DateTime(2023, 11, 2, 16, 28, 32)),
            new User("seba", new DateTime(2023, 11, 5, 9, 1, 44)),
        };

        users[1].Friends.Add(users[2]);
        users[1].Friends.Add(users[3]);
        users[1].Friends.Add(users[6]);
        users[2].Friends.Add(users[1]);
        users[3].Friends.Add(users[1]);
        users[3].Friends.Add(users[6]);
        users[4].Friends.Add(users[5]);
        users[5].Friends.Add(users[4]);
        users[6].Friends.Add(users[1]);
        users[6].Friends.Add(users[3]);
        return users;
    }
}
