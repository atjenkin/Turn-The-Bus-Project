using UnityEngine;

public class lr_LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetUpLine(Transform[] points)
    {
        Vector3[] positions = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            positions[i] = points[i].position;
        }
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    public void UpdateLine(Transform[] points)
    {
        Vector3[] positions = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            positions[i] = points[i].position;
        }
        lineRenderer.SetPositions(positions);
    }
}
