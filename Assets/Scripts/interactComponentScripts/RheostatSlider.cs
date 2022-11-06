using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RheostatSlider : MonoBehaviour
{
    private Camera mainCamera;
    public const float SLIDINGRANGE = 0.18f;
    private Vector3 originPostion;
    public float Ratio = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        originPostion = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void OnMouseDrag()
    {
        float cameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
        Vector3 screenPostion = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        Vector3 newWorldPostion = mainCamera.ScreenToWorldPoint(screenPostion);

        newWorldPostion.x = originPostion.x;
        newWorldPostion.y = originPostion.y;
        newWorldPostion.z = Math.Max(originPostion.z-SLIDINGRANGE, newWorldPostion.z);
        newWorldPostion.z = Math.Min(originPostion.z+SLIDINGRANGE, newWorldPostion.z);

        transform.position = newWorldPostion;
        
        float newRatio = 1 - (newWorldPostion.z-originPostion.z+SLIDINGRANGE) / (2*SLIDINGRANGE);
        newRatio = Math.Max(newRatio, 0);
        newRatio = Math.Min(newRatio, 1);
        Ratio = newRatio;
    }
}
