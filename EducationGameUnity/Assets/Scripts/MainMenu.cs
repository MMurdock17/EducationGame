using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void CreateSetScene()
	{
		
		SceneManager.LoadSceneAsync(7);
	}

	public void LoadGame()
	{
		SceneManager.LoadSceneAsync(14);
	}

}
