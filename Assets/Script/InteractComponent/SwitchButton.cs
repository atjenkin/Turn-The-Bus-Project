using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    public bool PlugIn = false;
    public Material ON;
    public Material OFF;

    void OnMouseDown()
    {
        PlugIn = !PlugIn;
        if( PlugIn) { gameObject.GetComponent<Renderer>().material = ON; }
        else { gameObject.GetComponent<Renderer>().material = OFF; }
    }
}
