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

        string user = username.text.Trim();
        string pass = password.text.Trim();

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            prompt.text = "Username and password required.";
            return;
        }

        dbReference.Child("users").Child(user).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
                return;
            }
            DataSnapshot snapshot = task.Result;

            // if username taken
            if (snapshot.Exists)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    prompt.text = "Username already exists. Please choose a different username or log in.";
                });
            }
            // if username available
            else
            {
                string hashed = PasswordHashing.Hash(pass);
                User newUser = new User(user, hashed);
                string json = JsonUtility.ToJson(newUser);

                dbReference.Child("users").Child(user).SetRawJsonValueAsync(json).ContinueWith(setTask =>
                {
                    if (setTask.IsFaulted)
                    {
                        Debug.LogError(setTask.Exception);
                    }
                    else
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            prompt.text = "Account create successfully!";
                        });
                    }
                });
            }
        });

    }

    public void LogIn()
    {
    StartCoroutine(LogInRoutine());
    }

    private IEnumerator LogInRoutine()
    {
        string typedUsername = username.text;
        string typedPassword = password.text;

        

        
        var usersTask = dbReference.Child("users").GetValueAsync();

        yield return new WaitUntil(() => usersTask.IsCompleted);

        if (usersTask.Exception != null)
        {
            prompt.text = "Database error.";
            yield break;
        }

        DataSnapshot usersSnapshot = usersTask.Result;

        bool usernameFound = false;

        foreach (var user in usersSnapshot.Children)
        {
            string dbUsername = user.Child("username").Value?.ToString();
            string dbPasswordHash = user.Child("password").Value?.ToString();

            
            if (dbUsername == typedUsername)
            {
                usernameFound = true;

                string typedHash = PasswordHashing.Hash(typedPassword);

                if (typedHash == dbPasswordHash)
                {
                    prompt.text = "Login successful!";
                    
                    yield break;
                }
                else
                {
                    prompt.text = "Incorrect password.";
                    yield break;
                }
            }
        }

        if (!usernameFound)
        {
            prompt.text = "No account found with that username.";
        }
    }
    
}
