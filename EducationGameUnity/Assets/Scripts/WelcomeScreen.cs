using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeScreen : MonoBehaviour
{
    
    public void LoadSignUp()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadLogIn()
    {
        SceneManager.LoadSceneAsync(2);
    }

}
