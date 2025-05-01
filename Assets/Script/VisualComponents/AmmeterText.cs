using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmeterText : MonoBehaviour
{
    [SerializeField] TextMeshPro AmmeterValue;

    public void UpdateAmmeterValue(double ammeterValue)
    {
        if (ammeterValue >= 100 || ammeterValue <= -100)
        {
            AmmeterValue.text = string.Format("{0:0.##}", ammeterValue / 1000) + " A";
        }
        else if (ammeterValue > 0.001 || ammeterValue < -0.001)
        {
            AmmeterValue.text = string.Format("{0:0.##}", ammeterValue) + " mA";
        }
        else
        {
            AmmeterValue.text = string.Format("{0:0.##}", ammeterValue * 1e3) + " \u03bcA";
        }
    }

    public void InitAmmeterValue()
    {
        AmmeterValue = GetComponent<TextMeshPro>();
    }
}
