using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] public float time;
    public Text TimerText;
    public Image Fill;
    public float Max;

    private bool hasTriggeredDefeat = false; // Évite les appels multiples

    void Update()
    {
        time -= Time.deltaTime;
        TimerText.text = "" + (int)time;
        Fill.fillAmount = time / Max;

        if (time <= 0 && !hasTriggeredDefeat)
        {
            hasTriggeredDefeat = true;
            time = 0;

            // Utilise la transition pour charger la scène de défaite (index 3)
            if (SceneTransition.Instance != null)
            {
                SceneTransition.Instance.LoadSceneWithTransition(3);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(3); // Fallback
            }
        }
    }
}