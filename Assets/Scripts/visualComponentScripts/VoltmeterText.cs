using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VoltmeterText : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] TextMeshPro mVoltageValue;


    public void updateVoltageValue(double voltage){
        mVoltageValue.text = voltage + " mV";
    }
    
    void Start()
    {
        mVoltageValue = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        // updateVoltageValue(2.0);
    }


}
