using UnityEngine;

public class MinimapFollow : MonoBehaviour
{

    [SerializeField]
    GameObject lookAt;
    [SerializeField]
    GameObject boatPlane;
    LineRenderer lineRenderer;

    [SerializeField]
    RectTransform playerMarker;

    void Start()
    {
        lineRenderer = boatPlane.GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        MoveCamera();
        DrawRenderer();
        RotateMarker();
    }

    void RotateMarker()
    {
        playerMarker.localEulerAngles = new Vector3(0,0,-lookAt.transform.localEulerAngles.y+180);
    }

    void MoveCamera()
    {
        transform.position = new Vector3(lookAt.transform.position.x,transform.position.y,lookAt.transform.position.z);
    }

    void DrawRenderer()
    {
        Vector3[] positions = new Vector3[2];

        positions[0] = lookAt.transform.position;
        positions[1] = boatPlane.transform.position;

        lineRenderer.SetPositions(positions);
    }

}
