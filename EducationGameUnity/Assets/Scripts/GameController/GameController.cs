using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public TMP_Text questionText;
    public TMP_Text answerButton1Text;
    public TMP_Text answerButton2Text;
    public TMP_Text feedbackText;

    public Button answerButton1;
    public Button answerButton2;

    private StudySet loadedSet;
    private int currentIndex = 0;
    private string correctAnswer;

    void Start()
    {
        LoadSet();
        if (loadedSet == null || loadedSet.cards == null || loadedSet.cards.Count == 0)
        {
            Debug.LogError("No valid cards to display!");
            
            SceneManager.LoadSceneAsync(3);
            return;
        }

        ShowQuestion();
    }

    void LoadSet()
    {
        string json = PlayerPrefs.GetString("SavedSet");

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("No saved set found!");
            return;
        }

        loadedSet = JsonUtility.FromJson<StudySet>(json);

        if (loadedSet == null)
        {
            Debug.LogError("Failed to deserialize JSON!");
            return;
        }

        if (loadedSet.cards == null || loadedSet.cards.Count == 0)
        {
            Debug.LogError("Loaded set has no cards!");
        }
        else
        {
            Debug.Log("Loaded " + loadedSet.cards.Count + " cards successfully.");
        }
    }

    void ShowQuestion()
    {
        feedbackText.text = "";

        
        if (loadedSet == null || loadedSet.cards == null || loadedSet.cards.Count == 0)
            return;

        if (currentIndex >= loadedSet.cards.Count)
        {
            SceneManager.LoadSceneAsync(3);
            return; 
        }

        Flashcard q = loadedSet.cards[currentIndex];

        questionText.text = q.question;
        correctAnswer = q.answer;

        string wrongAnswer = GetRandomWrongAnswer(q.answer);

        if (Random.Range(0, 2) == 0)
        {
            answerButton1Text.text = correctAnswer;
            answerButton2Text.text = wrongAnswer;
        }
        else
        {
            answerButton1Text.text = wrongAnswer;
            answerButton2Text.text = correctAnswer;
        }

        answerButton1.onClick.RemoveAllListeners();
        answerButton2.onClick.RemoveAllListeners();

        answerButton1.onClick.AddListener(() => CheckAnswer(answerButton1Text.text));
        answerButton2.onClick.AddListener(() => CheckAnswer(answerButton2Text.text));
    }

    string GetRandomWrongAnswer(string correct)
    {
        List<string> possible = new List<string>();

        foreach (var q in loadedSet.cards)
        {
            if (q.answer != correct)
                possible.Add(q.answer);
        }

        if (possible.Count == 0)
            return "No wrong answers available";

        return possible[Random.Range(0, possible.Count)];
    }

    void CheckAnswer(string chosen)
    {
        feedbackText.text = chosen == correctAnswer ? "Good job!" : "That is incorrect.";
        Invoke("NextQuestion", 1f);
    }

    void NextQuestion()
    {
        currentIndex++;
        ShowQuestion();
    }
}
