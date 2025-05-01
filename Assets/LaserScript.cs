using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(LineRenderer))]
public class LaserScript : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask layerMask;
    public float defaultLength = 50;


    private LineRenderer _lineRenderer;
    private RaycastHit   hit;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        _lineRenderer.SetPosition(0, transform.position);

        // Does the ray intersect any objects
        if (Physics.Raycast(transform.position, transform.forward, out hit, defaultLength, layerMask))
        {
            _lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            _lineRenderer.SetPosition(1, transform.position + (transform.forward * defaultLength));
        }
    }
}