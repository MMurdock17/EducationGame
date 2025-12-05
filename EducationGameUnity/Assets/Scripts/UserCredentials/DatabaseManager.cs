using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class DatabaseManager : MonoBehaviour
{

    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_Text usernameText;
    public TMP_Text passwordText;

    private string userID;
    private DatabaseReference dbReference;

    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateUser()
    {

        User newUser = new User(username.text, password.text);
        string json = JsonUtility.ToJson(newUser);

        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }

    public IEnumerator GetUsername(Action<string> onCallback)
    {
        var usernameData = dbReference.Child("users").Child(userID).Child("username").GetValueAsync();

        yield return new WaitUntil(predicate: () => usernameData.IsCompleted);

        if (usernameData != null)
        {
            DataSnapshot snapshot = usernameData.Result;

            onCallback.Invoke(snapshot.Value.ToString());
        }
    }

    /*
    public IEnumerator GetPassword(Action<string> onCallback)
    {
        var userPasswordData = dbReference.Child("users").Child(userID).Child("password").GetValueAsync();

        yield return new WaitUntil(predicate: () => userPasswordData.IsCompleted);

        if (userPasswordData != null)
        {
            DataSnapshot snapshot = userPasswordData.Result;

            onCallback.Invoke(snapshot.Value.ToString());
        }
    }

    public void GetUserInfo()
    {
        StartCoroutine(GetEmail((string email) => {
            emailText.text = "Email: " + email;

        }));

        StartCoroutine(GetPassword((string password) => {
            passwordText.text = "Password: " + password;

        }));
    }
    */
    
}
