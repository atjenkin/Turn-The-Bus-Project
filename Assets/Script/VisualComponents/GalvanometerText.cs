using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GalvanometerText : MonoBehaviour
{
    [SerializeField] TextMeshPro GalvanometerValue;
    // [SerializeField] TextMeshPro GalvanometerResistanceValue;

    public void UpdateGalvanometerValue(double current)
    {
        if (current >= 100 || current <= -100) 
        {
            GalvanometerValue.text = string.Format("{0:0.##}", current/1000) + " A";
            // Debug.Log("The value of someValue is in A: " + currentText);
        }
        else 
        {
            GalvanometerValue.text = string.Format("{0:0.##}", current) + " mA";
            // Debug.Log("The value of someValue is in mA: " + currentText);
        }
        // GalvanometerValue.text = string.Format("{0:0.##}", current/20) + " div";
        
    
    
    }

    public void InitGalvanometerValue()
    {
        GalvanometerValue = GetComponent<TextMeshPro>();
        // GalvanometerResistanceValue = GetComponent<TextMeshPro>();
    }
}
