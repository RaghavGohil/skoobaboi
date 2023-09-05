using UnityEngine;

public class MinimapFollow : MonoBehaviour
{

    [SerializeField]
    Transform lookAt;
    [SerializeField]
    Transform boatPlane;
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
        playerMarker.localEulerAngles = new Vector3(0,0,-lookAt.localEulerAngles.y+180);
    }

    void MoveCamera()
    {
        transform.position = new Vector3(lookAt.position.x,transform.position.y,lookAt.position.z);
    }

    void DrawRenderer()
    {
        Vector3[] positions = new Vector3[2];

        positions[0] = lookAt.position;
        positions[1] = boatPlane.position;

        lineRenderer.SetPositions(positions);
    }

}
