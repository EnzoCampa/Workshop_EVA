using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public float time;
    public Text TimerText;
    public Image Fill;
    public float Max;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        TimerText.text = "" + (int)time;
        Fill.fillAmount = time / Max;

        if(time < 0)
        time = 0;
    }
}
