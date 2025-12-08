using UnityEngine;

[System.Serializable]
public class Flashcard
{
    public string question;
    public string answer;

    public Flashcard() { }

    public Flashcard(string question, string answer)
    {
        this.question = question;
        this.answer = answer;
    }
}
