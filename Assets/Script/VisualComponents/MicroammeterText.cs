using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MicroammeterText : MonoBehaviour
{
    [SerializeField] TextMeshPro MicroammeterValue;

    public void UpdateAmmeterValue(double microammeterValue)
    {
        if (microammeterValue >= 100000 || microammeterValue <= -100000)
        {
            MicroammeterValue.text = string.Format("{0:0.##}", microammeterValue / 1000000) + " A";
        }
        else if (microammeterValue >= 100 || microammeterValue <= -100)
        {
            MicroammeterValue.text = string.Format("{0:0.##}", microammeterValue / 1000) + " mA";
        }
        else
        {
            MicroammeterValue.text = string.Format("{0:0.##}", microammeterValue) + " µA";
        }
    }

    public void InitAmmeterValue()
    {
        MicroammeterValue = GetComponent<TextMeshPro>();
    }
}
