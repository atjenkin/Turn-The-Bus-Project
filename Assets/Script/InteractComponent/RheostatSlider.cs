using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RheostatSlider : MonoBehaviour
{
    private Camera mainCamera;

    public double Ratio = 0.5f;
    public double PathLength;
    public Collider rodCollider;
    public GameObject EndPointLeft;
    public GameObject EndPointRight;

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
        PathLength = Vector3.Distance(EndPointLeft.transform.position, EndPointRight.transform.position);
    }

    private void OnMouseDrag()
    {
        float cameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        Vector3 newWorldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        Vector3 closestPoint = rodCollider.ClosestPoint(newWorldPosition);
        transform.position = closestPoint;

        double sliderLength = Vector3.Distance(closestPoint, EndPointLeft.transform.position);
        double newRatio = sliderLength / PathLength;
        newRatio = Math.Max(newRatio, 0);
        newRatio = Math.Min(newRatio, 1);
        Ratio = newRatio;
    }
}
