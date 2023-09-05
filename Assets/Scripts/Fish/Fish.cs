using UnityEngine;

[CreateAssetMenu(fileName="Fish",menuName="FishSO")]
public class Fish : ScriptableObject
{
    public GameObject fishPrefab;
    public int numFishes;
    public int[] levelGenerated; // levels on which the fish will be generated.
    public float speed;

    public float rotationSpeed;

    public float collisionDetectRayDistance;

    public float collisionDetectRayOffset;

    [Header("For hostile fishes")]

    public float viewRangeRadius;
    public float attackRangeRadius;
}
