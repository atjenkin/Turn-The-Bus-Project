using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VoltmeterText : MonoBehaviour
{
    [SerializeField] TextMeshPro VoltageValue;

    public void UpdateVoltageValue(double voltage)
    {
        if (voltage >= 100 || voltage <= -100) { VoltageValue.text = string.Format("{0:0.##}", voltage / 1000) + " V"; }
        else { VoltageValue.text = string.Format("{0:0.##}", voltage) + " mV"; }
    }

    public void InitVoltageValue()
    {
        VoltageValue = GetComponent<TextMeshPro>();
    }
}
