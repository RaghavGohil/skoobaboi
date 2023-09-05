using UnityEngine;
using Game.Helpers;

public class FishAI : MonoBehaviour
{
    RaycastHit hitCenter;
    RaycastHit hitLeft;
    RaycastHit hitRight;

    int withoutPlayerMask;
    int playerMaskIndex; // to exclude the player from the raycast physics

    FishData fishData;
    int randomRotationClamp;
    int randomTime;
    int randomRotationDuration;
    int randomRotationDirection;

    int directionalCheckRotateDegree;

    float currentTime;

    bool canRotateRandomly;

    void Start()
    {
        AvoidPlayer();
        fishData = GetComponent<FishData>();
        canRotateRandomly = true;
        randomRotationClamp = 5;
        randomTime = Random.Range(2,randomRotationClamp);
        randomRotationDirection = Random.Range(0,2);
        currentTime = 0f;
        randomRotationDuration = 5;
        directionalCheckRotateDegree = 45;
    }

    void FixedUpdate()
    {
        if(GameManagerOld.instance.isDiving)
        {
            MoveForward();
            CheckHit();
            if(canRotateRandomly)
                RotateRandomly();
        }
    }

    void AvoidPlayer()
    {
        playerMaskIndex = LayerMask.NameToLayer("Player");
        withoutPlayerMask = 1 << playerMaskIndex;
        withoutPlayerMask = ~withoutPlayerMask;
    }

    void MoveForward()
    {
        transform.position += -1 * transform.right * Time.deltaTime * fishData.fish.speed;
    }

    void RotateRandomly()
    {
        currentTime+=Time.deltaTime;
        if(currentTime > randomTime)
        {   
            if(randomRotationDirection == 1)
                transform.Rotate(0f,fishData.fish.rotationSpeed*Time.deltaTime,0f,Space.World);
            else
                transform.Rotate(0f,-fishData.fish.rotationSpeed*Time.deltaTime,0f,Space.World);
            if((currentTime-randomTime)>randomRotationDuration) // after 3 seconds stop the rotation
            {
                currentTime = 0f;
                randomRotationDirection = Random.Range(0,2);
            }
        }
    }

    void CheckHit()
    {

        Vector3 startPos,centerCheckEndPos,leftCheckEndPos,rightCheckEndPos;

        if(transform.childCount>0)
            startPos = transform.GetChild(0).transform.position;
        else
            startPos = transform.position;

        centerCheckEndPos = -transform.right * fishData.fish.collisionDetectRayDistance;
        leftCheckEndPos = Quaternion.Euler(0,-directionalCheckRotateDegree,0) * centerCheckEndPos; // rotate the vector
        rightCheckEndPos = Quaternion.Euler(0,directionalCheckRotateDegree,0) * centerCheckEndPos;

        float rotationAmount = 0f;
        Vector3 crossProduct = Vector3.zero;

        bool isCollidingLeft = Physics.Raycast(startPos, leftCheckEndPos, out hitLeft, fishData.fish.collisionDetectRayDistance , withoutPlayerMask);
        bool isCollidingRight = Physics.Raycast(startPos, rightCheckEndPos, out hitRight, fishData.fish.collisionDetectRayDistance, withoutPlayerMask);

        if(isCollidingRight && isCollidingLeft)
        {
            canRotateRandomly = false;
            Helpers.De_Ray(startPos, leftCheckEndPos, Color.red);
            Helpers.De_Ray(startPos, rightCheckEndPos, Color.red);
        }

        else if(isCollidingLeft)
        {
            Helpers.De_Ray(startPos, leftCheckEndPos, Color.red);
            Helpers.De_Ray(startPos, rightCheckEndPos, Color.yellow);
            randomRotationDirection = 1;
        }

        else if(isCollidingRight)
        {
            randomRotationDirection = 0;
            Helpers.De_Ray(startPos, rightCheckEndPos, Color.red);
            Helpers.De_Ray(startPos, leftCheckEndPos, Color.yellow);
        }

        else
        {
            Helpers.De_Ray(startPos, leftCheckEndPos, Color.yellow);
            Helpers.De_Ray(startPos, rightCheckEndPos, Color.yellow);
        }

        if(Physics.Raycast(startPos, centerCheckEndPos, out hitCenter, fishData.fish.collisionDetectRayDistance, withoutPlayerMask)) // center checks the left and right.
        {
            Helpers.De_Ray(startPos, centerCheckEndPos, Color.red);

            canRotateRandomly = false;

            rotationAmount = fishData.fish.rotationSpeed*Time.deltaTime;
            // to turn left or right?
            Vector3 surfaceNormal = hitCenter.normal;

            // if cross is positive then turn right else turn left
            crossProduct = Vector3.Cross(surfaceNormal,transform.right);
            float dot = Vector3.Dot(surfaceNormal,transform.right);
            if(dot == 0)
                transform.Rotate(0f,rotationAmount*5,0f,Space.World);
        }
        else
        {
            canRotateRandomly = true;
            Helpers.De_Ray(startPos, centerCheckEndPos, Color.yellow);
        }

        if(crossProduct.y != 0 && !canRotateRandomly)
        {
            if(crossProduct.y > 0)
                transform.Rotate(0f,rotationAmount,0f,Space.World);
            else
                transform.Rotate(0f,-rotationAmount,0f,Space.World);
        }
    }
}
