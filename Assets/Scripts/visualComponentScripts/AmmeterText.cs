using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmeterText : MonoBehaviour
{
   
     [SerializeField] TextMeshPro mAmmeterValue;


    public void updateAmmeterValue(double ammeterValue){
        mAmmeterValue.text = ammeterValue + " mA";
    }
    
    // Start is called before the first frame update
    void Start()
    {
        mAmmeterValue = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        // updateAmmeterValue(2.0);
    }
}
