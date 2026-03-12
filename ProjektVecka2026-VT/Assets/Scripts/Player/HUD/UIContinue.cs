using UnityEngine;
using UnityEngine.SceneManagement;

public class UIContinue : UIFindPlayer
{
    public void Continue()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ReturnToTitleScreen()
    {
        Invoke("NewScene", 0.5f);
    }

    public void NewScene()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
