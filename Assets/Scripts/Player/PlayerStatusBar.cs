using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusBar : MonoBehaviour
{

    [SerializeField]
    protected Slider statusBar;
    [SerializeField]
    protected float sliderMaxValue,sliderValue;

    protected void Start()
    {
        sliderMaxValue = sliderValue = 1000f;
    }
    
    protected void DecreaseAmount(float amount)
    {
        if(statusBar != null)
        {
            if(sliderValue > 0)
            {
                sliderValue -= amount;
                statusBar.value = Mathf.Lerp(0,1,(sliderValue/sliderMaxValue));
            }
        }
    }

    protected void IncreaseAmount(float amount)
    {
        if(statusBar != null)
        {
            if(sliderValue < sliderMaxValue)
            {
                sliderValue += amount;
                statusBar.value = Mathf.Lerp(0,1,(sliderValue/sliderMaxValue));
            }
        }
    }

    protected virtual void ZeroValue() // if the value of the slider is 0.
    {
        //override
    }

}
