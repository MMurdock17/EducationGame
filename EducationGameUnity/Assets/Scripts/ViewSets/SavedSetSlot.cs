using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SavedSetSlot : MonoBehaviour
{
    public TMP_Text setNameText;
    public Button editButton;
    public Button deleteButton;
    public Button playButton;

    public void LoadSlot(string setName, System.Action onEdit, System.Action onDelete, System.Action onPlay)
    {
        setNameText.text = setName;

        editButton.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();

        editButton.onClick.AddListener(() => onEdit?.Invoke());
        deleteButton.onClick.AddListener(() => onDelete?.Invoke());
        playButton.onClick.AddListener(() => onPlay?.Invoke());

        gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        setNameText.text = "Empty";
        gameObject.SetActive(false);
    }
}
