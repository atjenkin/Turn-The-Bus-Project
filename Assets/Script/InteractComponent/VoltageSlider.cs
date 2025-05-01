using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class VoltageSlider : MonoBehaviour
{
    private Camera mainCamera;

    public float Voltage; // Current voltage value
    public float MaxVoltage = 15.0f; // Maximum voltage value
    public float MinVoltage = 0.0f; // Minimum voltage value
    public double Ratio = 0.5f;
    public double PathLength;
    public Collider rodCollider;
    public GameObject EndPointLeft;
    public GameObject EndPointRight;
    public TextMeshPro VoltageText;
    private Vector3 initialPosition;

    private void Start()
    {
        mainCamera = Camera.main;
        PathLength = Vector3.Distance(EndPointLeft.transform.position, EndPointRight.transform.position);
        initialPosition = EndPointLeft.transform.position;
        transform.position = initialPosition;

        UpdateVoltageText();
    }

    private void OnMouseDrag()
    {
        float cameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        Vector3 newWorldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        Vector3 closestPoint = rodCollider.ClosestPoint(newWorldPosition);
        Debug.Log("Closest Point: " + closestPoint);
        Debug.Log("EndPointLeft: " + EndPointLeft.transform.position);
        Debug.Log("EndPointRight: " + EndPointRight.transform.position);

        // Clamp the closest point to be between the endpoints
        Vector3 clampedPoint = new Vector3(
            Mathf.Clamp(closestPoint.x, EndPointLeft.transform.position.x, EndPointRight.transform.position.x),
            EndPointLeft.transform.position.y, // Ensuring the slider stays at the same Y position
            EndPointLeft.transform.position.z  // Ensuring the slider stays at the same Z position
        );

        transform.position = clampedPoint;

        float sliderLength = Vector3.Distance(clampedPoint, EndPointLeft.transform.position);
        float ratio = (float)(sliderLength / PathLength);
        ratio = Mathf.Clamp01(ratio);

        Voltage = Mathf.Lerp(MinVoltage, MaxVoltage, ratio);
        UpdateVoltageText();
    }
    private void UpdateVoltageText()
    {
        VoltageText.text = "Voltage: " + Voltage.ToString("F2") + "V";
    }
}    