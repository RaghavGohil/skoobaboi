using UnityEngine;
using TMPro;

public class SpeedRunTimer : MonoBehaviour
{
    
    [SerializeField]
    TMP_Text timerText;

    float minutes,seconds;

    bool showUIAnimation;

    void Start()
    {
        timerText.text = "";
        seconds = minutes = 0;
        showUIAnimation = true;
    }

    void FixedUpdate()
    {
        if(GameManager.instance.isDiving)
        {
            CalculateTime();
            UpdateTime();
            if(showUIAnimation)
                ShowAnimation();
        }

    }

    void ShowAnimation()
    {
        timerText.transform.GetComponent<Animator>().Play("SpeedRunTimer");
        showUIAnimation = false;
    }

    void CalculateTime()
    {
        seconds += Time.deltaTime;
        if(seconds > 59.99f)
        {
            minutes++;
            seconds = 0f;
        }
    }

    void UpdateTime()
    {
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
