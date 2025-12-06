using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class StudySetBuilder : MonoBehaviour
{
    public TMP_InputField questionField;
    public TMP_InputField answerField;

    private List<Flashcard> cards = new List<Flashcard>();
    private int currentIndex = -1;

    public void NextQuestion()
    {
        string q = questionField.text;
        string a = answerField.text;

        if (string.IsNullOrWhiteSpace(q) || string.IsNullOrWhiteSpace(a))
        {
            return;
        }
        Flashcard card = new Flashcard(q, a);

        if (currentIndex == cards.Count - 1)
        {
            cards.Add(card);
        }
        else
        {
            cards[currentIndex] = card;
        }
        currentIndex++;
        questionField.text = "";
        answerField.text = "";
    }

    public void Back()
    {
        if (currentIndex <= 0)
        {
            return;
        }
        currentIndex--;
        questionField.text = cards[currentIndex].question;
        answerField.text = cards[currentIndex].answer;
    }

    public void CreateSet()
    {
        TempSetStorage.cards = cards;
        SceneManager.LoadSceneAsync(10);
    }
}
