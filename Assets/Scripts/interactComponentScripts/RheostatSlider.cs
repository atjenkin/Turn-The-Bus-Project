using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RheostatSlider : MonoBehaviour
{
    private Camera mainCamera;
    public const float SLIDINGRANGE = 0.18f;
    private Vector3 originPostion;

    public double Ratio = 0.5f;
    public double PathLength;
    public Collider rodCollider;
    public GameObject EndPointLeft;
    public GameObject EndPointRight;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        originPostion = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        PathLength = Vector3.Distance(EndPointLeft.transform.position, EndPointRight.transform.position);
    }

    // Update is called once per frame
    void OnMouseDrag()
    {
        float cameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
        Vector3 screenPostion = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        Vector3 newWorldPostion = mainCamera.ScreenToWorldPoint(screenPostion);
        
        Vector3 closestPoint = rodCollider.ClosestPoint(newWorldPostion);
        transform.position = closestPoint;

        double slideLength = Vector3.Distance(closestPoint, EndPointLeft.transform.position);
        double newRatio = slideLength / PathLength;
        newRatio = Math.Max(newRatio, 0);
        newRatio = Math.Min(newRatio, 1);
        Ratio = newRatio;
    }
}
