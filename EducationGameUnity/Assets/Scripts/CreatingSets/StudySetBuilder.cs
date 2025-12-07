using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class StudySetBuilder : MonoBehaviour
{
    public TMP_InputField questionField;
    public TMP_InputField answerField;

    private List<Flashcard> cards = new List<Flashcard>();
    private int currentIndex = 0;

    void Start()
    {
        
        if (cards.Count == 0)
        {
            cards.Add(new Flashcard("", ""));
        }

        LoadCardIntoFields();
    }

    private void SaveCurrentCard()
    {
        cards[currentIndex].question = questionField.text;
        cards[currentIndex].answer = answerField.text;
    }

    private void LoadCardIntoFields()
    {
        questionField.text = cards[currentIndex].question;
        answerField.text = cards[currentIndex].answer;
    }

    public void NextQuestion()
    {
        SaveCurrentCard();

        
        if (currentIndex == cards.Count - 1)
        {
            cards.Add(new Flashcard("", ""));
        }

        currentIndex++;
        LoadCardIntoFields();
    }

    public void Back()
    {
        if (currentIndex == 0)
            return;

        SaveCurrentCard();
        currentIndex--;
        LoadCardIntoFields();
    }

    public void CreateSet()
    {
        SaveCurrentCard();

        TempSetStorage.cards = cards;
        SceneManager.LoadSceneAsync(9);
    }
}
