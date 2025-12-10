using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Utilise la transition pour charger la scène de victoire (index 2)
            if (SceneTransition.Instance != null)
            {
                SceneTransition.Instance.LoadSceneWithTransition(2);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(2); // Fallback
            }
        }
    }
}