using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] public float time;

    [SerializeField] private AudioSource timerSound;
    [SerializeField] private AudioClip timerSoundClip;

    public Text TimerText;
    public Image Fill;
    public float Max;
    public bool Btimer = true;


    private bool hasTriggeredDefeat = false; // Évite les appels multiples
    private void Start()
    {
        timerSound.clip = timerSoundClip;
    }
    void Update()
    {
        time -= Time.deltaTime;
        TimerText.text = "" + (int)time;
        Fill.fillAmount = time / Max;
        if (time <= 10 && Btimer)
        {
            timerSound.Play();
            Btimer = false;
        }
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