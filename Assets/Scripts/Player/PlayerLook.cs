using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerLook : MonoBehaviour
{

    float sens;

    [SerializeField]
    Transform player;

    float xrot;

    // Start is called before the first frame update
    void Start()
    {
        sens = 100f;
        xrot = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

        player.Rotate(Vector3.up*mouseX);

        xrot -= mouseY;

        xrot = Mathf.Clamp(xrot,-90f,90f);

        transform.localRotation = Quaternion.Euler(GameManager.instance.cameraRotation.x + xrot,GameManager.instance.cameraRotation.y,GameManager.instance.cameraRotation.z);
    }
}
