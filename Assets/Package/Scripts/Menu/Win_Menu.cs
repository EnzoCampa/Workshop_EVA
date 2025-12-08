using UnityEngine;
using UnityEngine.SceneManagement;

public class Win_Menu : MonoBehaviour
{
    public void Replay()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}

