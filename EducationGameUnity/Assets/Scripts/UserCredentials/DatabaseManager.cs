using UnityEngine;
using Firebase.Database;
using Firebase;
using Firebase.Auth;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class DatabaseManager : MonoBehaviour
{

    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_Text prompt;

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
    StartCoroutine(CreateUserRoutine());
    }

    private IEnumerator CreateUserRoutine()
    {
        string typedUsername = username.text;
        string typedPassword = password.text;

        if (string.IsNullOrWhiteSpace(typedUsername) || string.IsNullOrWhiteSpace(typedPassword))
        {
            prompt.text = "Username and password fields cannot be empty.";
            yield break;
        }

        // check if username already exists
        var usersTask = dbReference.Child("users").GetValueAsync();
        yield return new WaitUntil(() => usersTask.IsCompleted);

        if (usersTask.Exception != null)
        {
            prompt.text = "Database error.";
            yield break;
        }

        DataSnapshot snapshot = usersTask.Result;

        foreach (var user in snapshot.Children)
        {
            string existingUsername = user.Child("username").Value?.ToString();
            if (existingUsername == typedUsername)
            {
                prompt.text = "Username already taken!";
                yield break;
            }
        }

        // Username is new -- create account

        string hashedPassword = PasswordHashing.Hash(typedPassword);
        User newUser = new User(typedUsername, hashedPassword);
        string json = JsonUtility.ToJson(newUser);

        var saveTask = dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
        yield return new WaitUntil(() => saveTask.IsCompleted);

        if (saveTask.Exception != null)
        {
            prompt.text = "Account could not be created.";
            yield break;
        }

        // Load Main Menu
        SceneManager.LoadSceneAsync(3);
    }






    public void LogIn()
    {
    StartCoroutine(LogInRoutine());
    }

    private IEnumerator LogInRoutine()
    {
        string typedUsername = username.text;
        string typedPassword = password.text;

        prompt.text = "Checking...";

        var usersTask = dbReference.Child("users").GetValueAsync();
        yield return new WaitUntil(() => usersTask.IsCompleted);

        if (usersTask.Exception != null)
        {
            prompt.text = "Database error.";
            yield break;
        }

        DataSnapshot usersSnapshot = usersTask.Result;

        foreach (var user in usersSnapshot.Children)
        {
            string dbUsername = user.Child("username").Value?.ToString();
            string dbPasswordHash = user.Child("passwordHash").Value?.ToString();

            // check for username
            if (dbUsername == typedUsername)
            {
                string typedHash = PasswordHashing.Hash(typedPassword);

                Debug.Log("Typed Password: " + typedPassword);
                Debug.Log("Typed Hash: " + typedHash);
                Debug.Log("Stored DB Password: " + dbPasswordHash);

                if (typedHash == dbPasswordHash)
                {
                    // Load Main Menu
                    SceneManager.LoadSceneAsync(3);
                    yield break;
                }

                if (dbPasswordHash == typedPassword) 
                {
                    // upgrade user to hashed password
                    string newHash = PasswordHashing.Hash(typedPassword);
                    dbReference.Child("users").Child(user.Key).Child("password").SetValueAsync(newHash);

                    SceneManager.LoadSceneAsync(3);
                    yield break;
                }

                else
                {
                    prompt.text = "Incorrect password.";
                    yield break;
                }
            }
        }

        prompt.text = "No account found with that username.";
    }
    
}
