using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;

public class User
{

    public string username;
    public string passwordHash;

    public User() {}

    public User(string username, string passwordHash)
    {
        this.username = username;
        this.passwordHash = passwordHash;
    }

}
