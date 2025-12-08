using UnityEngine;
using System.Collections.Generic;

public class SetData : MonoBehaviour
{
    public string setName;
    public List<QuestionData> questions = new List<QuestionData>();
}

public class QuestionData
{
    public string question;
    public string answer;

}