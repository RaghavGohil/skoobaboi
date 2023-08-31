using UnityEngine;
using UnityEngine.UI;
using Game.UI;

public class Oxygen : PlayerStatusBar
{

    enum OxygenLevel
    {
        normal,
        low,
        medium,
        high,
        ultra,
    }

    OxygenLevel oxygenLevel;

    [SerializeField]
    float runOutTime;

    protected new void Start()
    {
        base.Start();
        oxygenLevel = OxygenLevel.normal;
        sliderMaxValue = 1000f;
        sliderValue = 1000f;
        runOutTime = 10f;
    }

    void Update()
    {
        DecreaseTank();
        ZeroValue();
    }

    void DecreaseTank()
    {
    
        if(GameManagerOld.instance.isDiving && runOutTime > 0)
        {

            float depletionRate = sliderMaxValue/runOutTime;

            float depletedOxygen = depletionRate * Time.deltaTime;

            DecreaseAmount(depletedOxygen);
        }

    }

    protected override void ZeroValue()
    {
        if(sliderValue < 0)
        {
            UIManager.instance.YouLoseUI();
        }
    }

}
