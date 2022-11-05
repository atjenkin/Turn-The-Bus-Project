using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RheostatSlider : MonoBehaviour
{
    private Camera mainCamera;
    public const float SLIDINGRANGE = 0.36f;
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

        newWorldPostion.y = originPostion.y;
        newWorldPostion.z = originPostion.z;
        newWorldPostion.x = Math.Max(originPostion.x-SLIDINGRANGE, newWorldPostion.x);
        newWorldPostion.x = Math.Min(originPostion.x+SLIDINGRANGE, newWorldPostion.x);

        transform.position = newWorldPostion;

        Ratio = 1 - (newWorldPostion.x-originPostion.x+SLIDINGRANGE) / (2*SLIDINGRANGE);
    }
}
