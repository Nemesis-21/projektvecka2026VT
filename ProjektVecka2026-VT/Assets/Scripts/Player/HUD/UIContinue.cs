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
        SceneManager.LoadSceneAsync(0);
    }
}
