using UnityEngine;
using TMPro;

public class Level : MonoBehaviour
{

    [SerializeField]
    private TMPro.TMP_Text levelStatus;

    [SerializeField]
    GameObject plane;

    static int level;

    public static Level instance;

    float numLevels;
    public float unit;

    void Awake()
    {
        instance = this;
        numLevels = 5f;
        unit = -plane.transform.position.y/numLevels;
    }

    void FixedUpdate()
    {
        if(GameManagerOld.instance.isDiving)
            CalculateLevel();
    }

    void CalculateLevel()
    {   
        for (int i = 1; i <= numLevels; i++)
        {
            if(-transform.position.y <= unit*i && -transform.position.y > unit*(i-1))
            {
                levelStatus.text = "Level "+i.ToString();
            } 
        }
    }

}
