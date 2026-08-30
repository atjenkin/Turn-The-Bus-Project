using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentMeasureText : MonoBehaviour
{
    [SerializeField] TextMeshPro AmmeterValue;

    public void UpdateAmmeterValue(double ammeterValue, double leastCount)
    {
        double currentValue = ammeterValue / 1000;
        if (Math.Abs(currentValue) >= leastCount)
        {
            double adjustedValue = Math.Floor(currentValue / leastCount) * leastCount;
            AmmeterValue.text = string.Format("{0:0.##}", adjustedValue) + " A";
        }
        else
        {
            AmmeterValue.text = "0 A";
        }
    }

    public void InitAmmeterValue()
    {
        AmmeterValue = GetComponent<TextMeshPro>();
    }
}
