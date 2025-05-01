using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WireBuilder;

public abstract class CircuitComponent : MonoBehaviour
{
    public string Name;
    public string[] Interfaces;
    public float[] Parameters;

    public string Title;
    public string Description;

    public List<WireConnector> connectors;
    public List<SpiceSharp.Entities.IEntity> spiceEntitys;

    private Camera mainCamera;
    public Circuit registeredCircuit;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            if(registeredCircuit != null)
            {
                registeredCircuit.DestroyWires();
                registeredCircuit.GenerateWires();
            }
        }
    }

    public abstract void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title,string description);

    public virtual void RegisterComponent(Circuit circuit)
    {
        registeredCircuit = circuit;
        foreach(SpiceSharp.Entities.IEntity entity in spiceEntitys)
        {
            circuit.Ckt.Add(entity);
        }
    }

    public virtual void InitInterfaces(string[] interfaces)
    {
        connectors = new List<WireConnector>();
        for(int i=0; i<interfaces.Length; i++)
        {
            var childObject = gameObject.transform.Find(string.Format("interface{0}", i));
            if(childObject == null) { Debug.Log(string.Format("{0} has no interface{1}", Name, i)); continue; }
            else { connectors.Add(childObject.GetComponent<WireConnector>()); }
        }
    }
}
