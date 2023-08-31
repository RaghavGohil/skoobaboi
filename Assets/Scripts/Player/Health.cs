using UnityEngine;
using UnityEngine.UI;

public sealed class Health : PlayerStatusBar
{

    enum HealthLevel
    {
        normal,
        low,
        medium,
        high,
        ultra,
    }

    HealthLevel healthLevel;

    public float health{get;private set;}

    protected new void Start()
    {
        base.Start();
        health = 1000f;
        healthLevel = HealthLevel.normal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {

    }
}
