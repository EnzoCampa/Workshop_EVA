using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Lance la scène de jeu (index 1) avec transition
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadSceneWithTransition(1);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
        }
    }

    public void QuitGame()
    {
        // Optionnel : Ajouter une transition avant de quitter
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}