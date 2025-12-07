using UnityEngine;
using TMPro;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveStudySet : MonoBehaviour
{
    public TMP_InputField setNameField;
    public TMP_Text prompt;

    private DatabaseReference db;

    void Start()
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveSet()
    {
        string setName = setNameField.text;

        List<Flashcard> cards = TempSetStorage.cards;

        if (string.IsNullOrWhiteSpace(setName))
        {
            prompt.text = "Name your study set";
            return;
        }

        if (cards == null || cards.Count == 0)
        {
            prompt.text = "Your set must contain at least one card.";
            return;
        }

        StudySet newSet = new StudySet(setName, cards);
        string json = JsonUtility.ToJson(newSet);

        string userID = SystemInfo.deviceUniqueIdentifier;

        db.Child("users").Child(userID).Child("sets").Push().SetRawJsonValueAsync(json);

        SceneManager.LoadSceneAsync(12);
       
    }
}
