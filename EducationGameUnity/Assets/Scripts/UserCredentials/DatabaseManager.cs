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

    private TMP_Text usernameText;
    private TMP_Text passwordText;
    private string userID;
    private DatabaseReference dbReference;

    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateUser()
    {

        string user = username.text.Trim();
        string pass = password.text.Trim();

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            return;
        }

        // Hashing password
        string hash = PasswordHashing.Hash(pass);

        // Create user
        User newUser = new User(user, hash);

        //JSON conversion
        string json = JsonUtility.ToJson(newUser);

        dbReference.Child("users").Child(user).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to create user: " + task.Exception);
            }
            else
            {
                Debug.Log("User created successfully!");
            }
        });

    }

    public void LogIn()
    {

        string user = username.text.Trim();
        string pass = password.text.Trim();

        string hashedInput = PasswordHashing.Hash(pass);

        dbReference.Child("users").Child(user).GetValueAsync().ContinueWith(task =>
        {
            if (!task.IsCompleted || task.Result == null)
            {
                return;
            }

            DataSnapshot snapshot = task.Result;

            if (!snapshot.Exists)
            {
                return;
            }

            string storedHash = snapshot.Child("passwordHash").Value.ToString();

            if (storedHash == hashedInput)
            {
                Debug.Log("Log in successful!");
            }
            else
            {
                Debug.Log("Incorrect password");
            }

        });

    }
    
}
