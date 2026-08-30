using UnityEngine;

public class RaySimulation : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;  // Assign in Inspector
    [SerializeField] private GameObject myobj;           // The object where the ray starts (Assign in Inspector)

    private Vector3 rayStartPoint;                       // Starting point of the ray
    private Vector3 rayEndPoint;                         // End point of the ray (extending to the right)

    void Start()
    {
        // Ensure the LineRenderer uses world space
        lineRenderer.useWorldSpace = true;
        Debug.Log("World space set to true for LineRenderer");

        // Check if the myobj and lineRenderer are correctly assigned
        if (myobj == null)
        {
            Debug.LogError("myobj (rayOriginObject) is not assigned! Please assign it in the Inspector.");
            return;
        }

        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer is not assigned! Please assign it in the Inspector.");
            return;
        }

        // Set the initial ray starting point to the object's position
        rayStartPoint = myobj.transform.position;
        Debug.Log("Ray starting point set to: " + rayStartPoint);

        // Extend the ray horizontally to the right by 5 units
        rayEndPoint = rayStartPoint + new Vector3(5, 0, 0);
        Debug.Log("Ray end point set to: " + rayEndPoint);

        // Draw the initial light ray
        DrawInitialRay();
    }

    void DrawInitialRay()
    {
        // Set the line renderer's position count to 2 (start and end)
        lineRenderer.positionCount = 2;
        Debug.Log("LineRenderer position count set to 2");

        lineRenderer.SetPosition(0, rayStartPoint);      // Start of the ray
        Debug.Log("LineRenderer start position set to: " + rayStartPoint);

        lineRenderer.SetPosition(1, rayEndPoint);        // End point of the ray
        Debug.Log("LineRenderer end position set to: " + rayEndPoint);
    }

    void Update()
    {
        // Update positions if the object moves
        rayStartPoint = myobj.transform.position;
        rayEndPoint = rayStartPoint + new Vector3(5, 0, 0);

        Debug.Log("Updated ray start point: " + rayStartPoint);
        Debug.Log("Updated ray end point: " + rayEndPoint);

        // Update the line positions in the line renderer
        lineRenderer.SetPosition(0, rayStartPoint);
        lineRenderer.SetPosition(1, rayEndPoint);

        Debug.Log("LineRenderer positions updated");
    }
}
