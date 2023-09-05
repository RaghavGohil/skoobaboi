// this script is responsible for decreasing health points if the player is way too close to a hostile fish
// the component is only attached to hostile fishes

using UnityEngine;
using Game.Helpers;

public sealed class FishAttack : MonoBehaviour
{
    int playerMaskIndex; // only mask out the player because only that's needed
    int playerMask;
    FishData fishData;
    FishAI fishAI;
    bool gotPlayer;
    Transform player;

    void Start()
    {
        gotPlayer = false;
        playerMaskIndex = LayerMask.NameToLayer("Player");
        playerMask = 1 << playerMaskIndex;
        fishData = GetComponent<FishData>();
        fishAI = GetComponent<FishAI>();
    }

    void FixedUpdate()
    {
        if(!gotPlayer)
            CheckPlayerInViewRange();
        else if(gotPlayer)
        {
            FollowPlayer();
            CheckPlayerInAttackRange();
        }
    }

    void CheckPlayerInViewRange()
    {

        Helpers.De_Sphere(transform.position, fishData.fish.viewRangeRadius * transform.forward, Color.red);

        Collider[] cols = Physics.OverlapSphere(transform.position, fishData.fish.viewRangeRadius);

        foreach(Collider col in cols)
        {
            if(col.CompareTag("Player"))
            {
                player = col.transform; 

                gotPlayer = true;
                if(fishAI != null)
                    fishAI.enabled = false;
            }
        }
    }

    void CheckPlayerInAttackRange()
    {
        Helpers.De_Sphere(transform.position, fishData.fish.viewRangeRadius * transform.right, Color.yellow);

        if(Physics.CheckSphere(transform.position, fishData.fish.attackRangeRadius , playerMask))
        {
            print("Player is in the attack range!");
        }
    }

    void FollowPlayer()
    {
        transform.position = Vector3.Slerp(transform.position , player.transform.position, Time.deltaTime);
        transform.rotation = Quaternion.LookRotation((player.transform.localPosition - transform.localPosition) , Vector3.up);
    }

    void Attack()
    {

    }

    void OnDisabled()
    {
        if(fishAI != null)
                fishAI.enabled = true;
    }
}
