using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance; // Singleton pour accès global

    public Animator transition; // Corrigé : Animator au lieu d'Animation
    public float transitionTime = 1.0f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre les scènes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Méthode publique pour charger une scène avec transition
    public void LoadSceneWithTransition(int sceneIndex)
    {
        StartCoroutine(LoadLevel(sceneIndex));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        // Lance l'animation de transition
        transition.SetTrigger("Start");

        // Attend la fin de l'animation
        yield return new WaitForSeconds(transitionTime);

        // Charge la scène
        SceneManager.LoadScene(levelIndex);
    }
}