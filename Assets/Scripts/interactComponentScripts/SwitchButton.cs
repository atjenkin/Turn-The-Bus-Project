using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    public bool PlugIn = false;
    
    void OnMouseDown() {
        PlugIn = !PlugIn;
    }
}
