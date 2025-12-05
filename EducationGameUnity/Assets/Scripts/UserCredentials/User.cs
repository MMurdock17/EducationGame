using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;

public class User : MonoBehaviour
{

    public string email;
    public string password;

    public User(string email, string password)
    {
        this.email = email;
        this.password = password;
    }

}
