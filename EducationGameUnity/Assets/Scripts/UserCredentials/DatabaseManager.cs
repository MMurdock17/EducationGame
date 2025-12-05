using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;

public class DatabaseManager : MonoBehaviour
{

    public TMP_InputField email;
    public TMP_InputField password;

    private string userID;
    private DatabaseReference dbReference;

    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateUser()
    {
        Debug.Log("Email field: " + email);
        Debug.Log("Password field: " + password);
        Debug.Log("dbReference: " + dbReference);
        Debug.Log("userID: " + userID);

        User newUser = new User(email.text, password.text);
        string json = JsonUtility.ToJson(newUser);

        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }
    
}
