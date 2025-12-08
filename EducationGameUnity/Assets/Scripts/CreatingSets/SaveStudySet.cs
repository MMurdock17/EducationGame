using UnityEngine;
using TMPro;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveStudySet : MonoBehaviour
{
    public TMP_InputField setNameField;
    public TMP_Text prompt;

    public void SaveSet()
    {
        string setName = setNameField.text;

        if (string.IsNullOrWhiteSpace(setName))
        {
            prompt.text = "Name your study set.";
            return;
        }

        List<Flashcard> cards = TempSetStorage.cards;

        if (cards == null || cards.Count == 0)
        {
            prompt.text = "Your set must contain at least one card.";
            return;
        }

        SetData data = new SetData();
        data.setName = setName;
        data.questions = new List<QuestionData>();

        foreach (var card in cards)
        {
            data.questions.Add(new QuestionData
            {
                question = card.question,
                answer = card.answer
            });
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SavedSet", json);
        PlayerPrefs.Save();

        SceneManager.LoadScene("MainMenu");
    }
}
