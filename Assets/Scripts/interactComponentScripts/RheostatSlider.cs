/*
* This file was developed by a team from Carnegie Mellon University as a part of the practicum project for Fall 2022 in collaboration with Turn The Bus.
* Authors: Adrian Jenkins, Harshit Maheshwari, and Ziniu Wan. (Carnegie Mellon University)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RheostatSlider : MonoBehaviour
{
    private Camera mainCamera;

    public double Ratio = 0.5;
    public double PathLength;
    public Collider rodCollider;
    public GameObject EndPointLeft;
    public GameObject EndPointRight;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
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
