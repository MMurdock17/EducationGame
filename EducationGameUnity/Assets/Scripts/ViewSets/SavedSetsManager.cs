using UnityEngine;
using System.Collections.Generic;
using Firebase.Database;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SavedSetsManager : MonoBehaviour
{
    [Header("UI")]
    public Transform contentParent;
    public GameObject setListItemPrefab;
    public TMP_Text statusText;

    private DatabaseReference db;
    private string userID;

    void Start()
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;
        userID = SystemInfo.deviceUniqueIdentifier;

        LoadSavedSets();
    }

    private void LoadSavedSets()
    {
        statusText.text = "Loading...";

        var task = db.Child("users").Child(userID).Child("studySets").GetValueAsync();
        task.ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Debug.LogError(t.Exception);
                statusText.text = "Error loading sets";
                return;
            }

            if (t.IsCompleted)
            {
                DataSnapshot snapshot = t.Result;

                foreach (Transform child in contentParent)
                {
                    Destroy(child.gameObject);
                }

                if (!snapshot.Exists)
                {
                    statusText.text = "No saved sets";
                    return;
                }

                statusText.text = "";

                foreach (var set in snapshot.Children)
                {
                    string setID = set.Key;
                    string setName = set.Child("name").Value.ToString();

                    CreateSetRow(setID, setName);
                }
            }
        });
    }

    private void CreateSetRow(string setID, string setName)
    {
        GameObject row = Instantiate(setListItemPrefab, contentParent);

        TMP_Text nameText = row.transform.Find("SetNameText").GetComponent<TMP_Text>();
        Button editButton = row.transform.Find("EditButton").GetComponent<Button>();
        Button deleteButton = row.transform.Find("DeleteButton").GetComponent<Button>();
        Button playButton = row.transform.Find("PlayButton").GetComponent<Button>();

        nameText.text = setName;

        editButton.onClick.AddListener(() => EditSet(setID));
        deleteButton.onClick.AddListener(() => DeleteSet(setID, row));
        playButton.onClick.AddListener(() => PlaySet(setID));
    }

    public void EditSet(string setID)
    {
        PlayerPrefs.SetString("editingSetID", setID);
        SceneManager.LoadSceneAsync(1); // put scene number
    }

    public void DeleteSet(string setID, GameObject row)
    {
        db.Child("users").Child(userID).Child("studySets").Child(setID).RemoveValueAsync();

        Destroy(row);

        if (contentParent.childCount == 0)
        {
            statusText.text = "No saved sets";
        }
    }

    public void PlaySet(string setID)
    {
        PlayerPrefs.SetString("playingSetID", setID);
        SceneManager.LoadSceneAsync(1); // put scene number
    }
}
