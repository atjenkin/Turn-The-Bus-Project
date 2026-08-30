using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WireExp8 : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    void Update()
    {
        if (pointA != null && pointB != null)
        {
            lr.SetPosition(0, pointA.position);
            lr.SetPosition(1, pointB.position);
        }
    }
}
