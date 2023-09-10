using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerMovement : MonoBehaviour
{
    
    Rigidbody rb;
    [SerializeField]
    
    float gravityMultiplier;
    float gravityScale;
    float horizontal,vertical;

    float moveSpeed;

    float frictionCoeff;

    [SerializeField]
    Transform playerCam;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = 10f;
        frictionCoeff = 1f;
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
    
    void FixedUpdate()
    {
        Vector3 gravityScale = Physics.gravity * gravityMultiplier;
        rb.AddForce(gravityScale, ForceMode.Acceleration);

        Vector3 friction = -rb.velocity * frictionCoeff;

        rb.AddForce(playerCam.transform.forward*vertical*moveSpeed*Time.fixedDeltaTime*100,ForceMode.Force);//movement to camera
        rb.AddForce(-1*transform.right*horizontal*moveSpeed*Time.fixedDeltaTime*100,ForceMode.Force); // horizontal movement
        rb.AddForce(friction*Time.fixedDeltaTime*100,ForceMode.Force);

    }
}
