using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase;
using Firebase.Auth;
using System.Collections.Generic;

public class SavedSetsManager : MonoBehaviour
{
    public TMP_Text loadingText;
    public SavedSetSlot[] slots;

    private void Start()
    {
        LoadSets();
    }

    private async void LoadSets()
    {
        loadingText.text = "Loading sets...";

        string userID = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        DatabaseReference db = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userID).Child("sets");

        var snapshot = await db.GetValueAsync();

        loadingText.gameObject.SetActive(false);

        foreach (var slot in slots)
            {
                slot.ClearSlot();
            }

        if (!snapshot.Exists)
        {
            Debug.Log("No sets found.");
            return;
        }

        int i = 0;

        foreach (var setSnap in snapshot.Children)
        {
            if (i >= slots.Length)
                {
                    break;
                }

            string setId = setSnap.Key;
            string setName = setSnap.Child("name").Value?.ToString() ?? "Unnamed Set";

            var slot = slots[i];

            slot.LoadSlot(setName,onEdit: () => EditSet(setId),onDelete: () => DeleteSet(setId),onPlay: () => PlaySet(setId));

            i++;
        }
    }

    private void EditSet(string id)
    {
        Debug.Log("Editing: " + id);
        // TODO: your logic
    }

    private void DeleteSet(string id)
    {
        Debug.Log("Deleting: " + id);
        // TODO: delete logic
    }

    private void PlaySet(string id)
    {
        Debug.Log("Play: " + id);
        // TODO: load set gameplay
    }
}
