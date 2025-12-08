using UnityEngine;

public static class SavedSetLoader
{
    public static StudySet LoadSavedSet()
    {
        string json = PlayerPrefs.GetString("SavedSet", "");

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("No saved set found!");
            return null;
        }

        return JsonUtility.FromJson<StudySet>(json);
    }
}
