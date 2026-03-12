using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame(int newScene)
    {
        if (SceneManager.sceneCount >= newScene) 
        { 
            SceneManager.LoadSceneAsync(newScene); 
        }
        else
        {
            print("could not load scene" + newScene);
        }
    }

    public void QuitGame() 
    {
        print("Quit game");
        Application.Quit(); 
    }

}
