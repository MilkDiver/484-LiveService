using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnStartButton()
    {
        //load next scene
        SceneManager.LoadScene(1);
    }

    //called when the Quit button is clicked
    public void OnQuitButton()
    {
        Application.Quit();

#if UNITY_EDITOR
        
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
