using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void CreateSetScene()
	{
		
		SceneManager.LoadSceneAsync(7);
	}

	public void SavedSetsScene()
	{
		SceneManager.LoadSceneAsync(12);
	}

	public void ChooseCharacter()
	{
		SceneManager.LoadSceneAsync(13);
	}

}
