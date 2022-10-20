using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CircuitComponent : MonoBehaviour
{   
    public string Name;
    public string[] Interfaces;
    public float[] Parameters;

    private Rigidbody rigidbodyComponent;
    protected SpiceSharp.Entities.IEntity spiceEntity;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>(); // shorthand for rigidbody component
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SpiceSharp.Entities.IEntity GetSpiceEntity()
    {
        return spiceEntity;
    }

    public abstract void InitSpiceEntity(string name, string[] interfaces, float[] parameters);
    public virtual void RegisterSimulation(SpiceSharp.Simulations.IBiasingSimulation sim)
    {

    }
}
