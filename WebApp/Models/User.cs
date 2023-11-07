using System.Text.Json.Serialization;

namespace WebApp.Models;

public class User
{
    public string Login { get; set; }
    [JsonIgnore]
    public List<User> Friends { get; set; }
    public DateTime Created { get; set; }
    public User()
    { 
        Friends = new List<User>();
    }
    public User(string login, DateTime created)
    {
        Login = login;
        Created = created;
        Friends = new List<User>();
    }
}
