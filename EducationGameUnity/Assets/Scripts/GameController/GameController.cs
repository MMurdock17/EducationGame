using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public TMP_Text questionText;
    public TMP_Text answerButton1Text;
    public TMP_Text answerButton2Text;
    public TMP_Text feedbackText;

    public Button answerButton1;
    public Button answerButton2;

    private SetData loadedSet;
    private int currentIndex = 0;
    private string correctAnswer;

    void Start()
    {
        LoadSet();
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

        loadedSet = JsonUtility.FromJson<SetData>(json);
    }

    void ShowQuestion()
    {
        feedbackText.text = "";

        if (currentIndex >= loadedSet.questions.Count)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            return;
        }

        QuestionData q = loadedSet.questions[currentIndex];

        // Set the question text
        questionText.text = q.question;
        correctAnswer = q.answer;

        // Get a random WRONG answer
        string wrongAnswer = GetRandomWrongAnswer(q.answer);

        // Randomize button placement
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

        // Assign button listeners
        answerButton1.onClick.RemoveAllListeners();
        answerButton2.onClick.RemoveAllListeners();

        answerButton1.onClick.AddListener(() => CheckAnswer(answerButton1Text.text));
        answerButton2.onClick.AddListener(() => CheckAnswer(answerButton2Text.text));
    }

    string GetRandomWrongAnswer(string correct)
    {
        List<string> possible = new List<string>();

        foreach (var q in loadedSet.questions)
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
        if (chosen == correctAnswer)
        {
            feedbackText.text = "Good job!";
        }
        else
        {
            feedbackText.text = "That is incorrect.";
        }

        // Move to next question after 1 second
        Invoke("NextQuestion", 1f);
    }

    void NextQuestion()
    {
        currentIndex++;
        ShowQuestion();
    }
}
