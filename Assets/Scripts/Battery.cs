using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    
    private Rigidbody rigidbodyComponent;
    public SpiceSharp.Components.VoltageSource voltageSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>(); // shorthand for rigidbody component
    }

    // Update is called once per frame
    void Update()
    {
        // Berkeley Spice
    }

    public void InitSpiceComponent(Dictionary<string, string> kwargs)
    {
        voltageSource = new SpiceSharp.Components.VoltageSource(kwargs["id"], kwargs["interface1"], kwargs["interface2"], 3.0);
    }
}
