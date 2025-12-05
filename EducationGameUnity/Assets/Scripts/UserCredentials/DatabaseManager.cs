using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour
{

    public InputField email;
    public InputField password;

    private string userID;
    private DatabaseReference dbReference;

    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateUser()
    {
        User newUser = new User(email.text, password.text);
        string json = JsonUtility.ToJson(newUser);

        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }
    
}
