// this script is responsible for decreasing health points if the player is way too close to a hostile fish
// the component is only attached to hostile fishes

using UnityEngine;
using Game.HelperFunctions;

public sealed class FishAttack : MonoBehaviour
{
    
    int playerMaskIndex; // only mask out the player because only that's needed
    int playerMask;

    FishData fishData;
    FishAI fishAI;
    
    bool gotPlayer;
    Transform player;

    const float rotSpeed = 100f;

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

        Helper.De_4DDirRay(transform.position, fishData.fish.viewRangeRadius, Color.red);

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
        Helper.De_4DDirRay(transform.localPosition, fishData.fish.attackRangeRadius, Color.yellow);

        if(Physics.CheckSphere(transform.localPosition, fishData.fish.attackRangeRadius , playerMask))
        {
            Attack();
        }
    }

    void FollowPlayer()
    {
        Helper.De_Ray(transform.position, Quaternion.Euler(0f, 90f, 0f) * -(transform.position - player.transform.position), Color.red);
        Helper.Log(player.transform.position + " " + transform.position);
        transform.position = Vector3.Slerp(transform.position, player.transform.position, Time.deltaTime);
        //Quaternion.Euler(0f,90f,0f)*(player.transform.position - transform.position)
        Quaternion desired_dir;
        desired_dir = Quaternion.LookRotation(player.transform.right, Vector3.up);
        if (transform.childCount == 0)
            desired_dir = desired_dir*Quaternion.Euler(-90f,0f,0f);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation,desired_dir,rotSpeed*Time.deltaTime);
    }

    void Attack()
    {
        print("Player is in the attack range!");
    }

    void OnDisabled()
    {
        if(fishAI != null)
                fishAI.enabled = true;
    }
}
