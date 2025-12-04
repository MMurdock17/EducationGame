using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void CreateSetScene()
	{
		
		SceneManager.LoadSceneAsync(8);
	}

	public void SavedSetsScene()
	{
		SceneManager.LoadSceneAsync(13);
	}

	public void ChooseCharacter()
	{
		SceneManager.LoadSceneAsync(15);
	}

}
