using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}