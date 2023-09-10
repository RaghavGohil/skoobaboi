using UnityEngine;
using TMPro;

public sealed class SpeedRunTimer : MonoBehaviour
{
    
    [SerializeField]
    TMP_Text timerText;

    internal float minutes,seconds;

    internal bool showUIAnimation;

    void Start()
    {
        timerText.text = "";
        seconds = minutes = 0;
        showUIAnimation = true;
    }

    void FixedUpdate()
    {
        if(GameManagerOld.instance != null)
        {
            if(GameManagerOld.instance.isDiving)
            {
                CalculateTime();
                UpdateTime();
                if(showUIAnimation)
                    ShowAnimation();
            }
        }

    }

    internal void ShowAnimation()
    {
        timerText.transform.GetComponent<Animator>().Play("SpeedRunTimer");
        showUIAnimation = false;
    }

    internal void CalculateTime()
    {
        seconds += Time.deltaTime;
        if(seconds > 59.99f)
        {
            minutes++;
            seconds = 0f;
        }
    }

    internal void UpdateTime()
    {
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
