using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using WireBuilder;

public class Circuit : MonoBehaviour
{
    /**************** JSON fields definition ****************/
    [System.Serializable]
    public class ComponentMeta
    {
        public string Name;
        public string Type;
        public float[] Position;
        public string[] Interfaces;
        public float[] Parameters;
    }

    [System.Serializable]
    public class ComponentMetaList
    {
        public ComponentMeta[] Components;
    }

    [System.Serializable]
    public class LabInfo
    {
        public string LabTitle;
        public string Aim;
        public string Background;
        public string Theory;
        public string Diagram;
    }

    [System.Serializable]
    public class LabMaterials
    {
        public string[] MaterialsRequired;
    }

    [System.Serializable]
    public class LabProcedure
    {
        public string[] Procedure;
    }

    [System.Serializable]
    public class LabObservations
    {
        public string[] Observations;
    }

    /**************** Members ****************/
    public static string labJSON;

    public List<CircuitComponent> circuitComponents;

    public SpiceSharp.Circuit Ckt;
    public SpiceSharp.Simulations.BiasingSimulation Sim;

    public ComponentMetaList componentMetaList = new ComponentMetaList();
    

    /**************** Methods ****************/
    // Start is called before the first frame update
    void Start()
    {
        TextAsset textJSON = Resources.Load<TextAsset>(labJSON);
        circuitComponents = new List<CircuitComponent>();
        componentMetaList = JsonUtility.FromJson<ComponentMetaList>(textJSON.text);

        InitCircuit();
        GenerateWires();
        RunCircuit();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitCircuit() 
    {
        Ckt = new SpiceSharp.Circuit();
        Sim = new SpiceSharp.Simulations.OP("Sim");
        foreach(ComponentMeta meta in componentMetaList.Components) 
        {
            string guid = AssetDatabase.FindAssets(meta.Type, new string[] {"Assets/Prefabs"})[0];
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            var instance = Instantiate(prefabObject, this.transform, true);
            instance.name = meta.Name;
            instance.transform.position = new Vector3(meta.Position[0], meta.Position[1], meta.Position[2]);

            CircuitComponent thisComponent = instance.GetComponent<CircuitComponent>();
            thisComponent.InitSpiceEntity(meta.Name, meta.Interfaces, meta.Parameters);

            circuitComponents.Add(thisComponent);
            thisComponent.RegisterComponent(this);

            thisComponent.InitInterfaces(meta.Interfaces);
        }
    }

    public void GenerateWires()
    {
        Dictionary<string, List<WireConnector>> interfaces = new Dictionary<string, List<WireConnector>>();
        foreach(CircuitComponent thisComponent in circuitComponents) 
        {
            for(int i=0; i<thisComponent.connectors.Count; i++)
            {
                string interfaceName = thisComponent.Interfaces[i];
                WireConnector connector = thisComponent.connectors[i];
                if(!interfaces.ContainsKey(interfaceName)) interfaces.Add(interfaceName, new List<WireConnector>());
                interfaces[interfaceName].Add(connector);
            }
        }
        foreach(var item in interfaces)
        {
            for(int i=1; i<item.Value.Count; i++) 
            {
                Wire wire = WireManager.CreateWireObject(item.Value[i-1], item.Value[i], item.Value[i].wireType);
                wire.transform.SetParent(this.transform);
            }
        }
    }

    public void RunCircuit()
    {
        Sim.Run(Ckt);
    }
}
