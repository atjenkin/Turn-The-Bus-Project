using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WireBuilder;

public abstract class CircuitComponent : MonoBehaviour
{   
    public string Name;
    public string[] Interfaces;
    public float[] Parameters;

    protected Rigidbody rigidbodyComponent;
    public SpiceSharp.Entities.IEntity spiceEntity;
    public Dictionary<string, WireConnector> connectors;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>(); // shorthand for rigidbody component
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void InitSpiceEntity(string name, string[] interfaces, float[] parameters);

    public virtual void RegisterComponent(Circuit circuit) {}

    public virtual void InitInterfaces(string[] interfaces)
    {
        connectors = new Dictionary<string, WireConnector>();
        for(int i=0; i<interfaces.Length; i++)
        {
            var childObject = gameObject.transform.Find(string.Format("interface{0}", i));
            if(childObject == null) {
                Debug.Log(string.Format("{0} has no interface{1}", Name, i));
                continue;
            } else {
                connectors.Add(interfaces[i], childObject.GetComponent<WireConnector>());
            }
        }
    }
}
