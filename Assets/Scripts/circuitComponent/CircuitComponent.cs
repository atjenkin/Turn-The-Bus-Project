using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CircuitComponent : MonoBehaviour
{   
    private Rigidbody rigidbodyComponent;
    public SpiceSharp.Entities.IEntity spiceEntity;
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
}
