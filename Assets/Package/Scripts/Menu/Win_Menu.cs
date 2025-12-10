using UnityEngine;
using UnityEngine.SceneManagement;

public class Win_Menu : MonoBehaviour
{
    public void Replay()
    {
        // Relance la scène de jeu (index 1) avec transition
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadSceneWithTransition(1);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
        }
    }

    public void MainMenu()
    {
        // Retour au menu principal (index 0) avec transition
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadSceneWithTransition(0);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
        }
    }
}