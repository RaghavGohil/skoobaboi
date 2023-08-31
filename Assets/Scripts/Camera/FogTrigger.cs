using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Upsurface"))
        {
            RenderSettings.fog = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Upsurface"))
        {
            RenderSettings.fog = true;
        }
    }
}
