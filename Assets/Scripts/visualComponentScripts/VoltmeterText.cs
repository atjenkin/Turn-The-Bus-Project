using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VoltmeterText : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] TextMeshPro VoltageValue;


    public void UpdateVoltageValue(double voltage){
        VoltageValue.text = string.Format("{0:0.##}", voltage) + "V";
    }
    
    public void InitVoltageValue()
    {
        VoltageValue = GetComponent<TextMeshPro>();   
    }
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // updateVoltageValue(2.0);
    }


}
