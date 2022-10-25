using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmeterText : MonoBehaviour
{
   
    [SerializeField]
    TextMeshPro AmmeterValue;


    public void UpdateAmmeterValue(double ammeterValue){
        AmmeterValue.text = string.Format("{0:0.##}", ammeterValue) + " mA";
    }

    public void InitAmmeterValue()
    {
        AmmeterValue = GetComponent<TextMeshPro>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // updateAmmeterValue(2.0);
    }
}
