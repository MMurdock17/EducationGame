using UnityEngine;
using System.Collections.Generic;

public class StudySet
{
    public string setName;
    public List<Flashcard> cards;

    public StudySet() { }

    public StudySet(string setName, List<Flashcard> cards)
    {
        this.setName = setName;
        this.cards = cards;
    }
}
