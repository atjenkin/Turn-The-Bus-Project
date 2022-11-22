using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLabel : MonoBehaviour
{
    void OnMouseDown(){
        Circuit.isLabelWindowOpen = false;
        Debug.Log("Label Window closed");
    }
}
