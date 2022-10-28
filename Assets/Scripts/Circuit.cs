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
            Ckt.Add(thisComponent.spiceEntity);
            thisComponent.RegisterComponent(this);

            thisComponent.InitInterfaces(meta.Interfaces);
        }
    }

    public void GenerateWires()
    {
        Dictionary<string, List<WireConnector>> interfaces = new Dictionary<string, List<WireConnector>>();
        foreach(CircuitComponent thisComponent in circuitComponents) 
        {
            foreach(var item in thisComponent.connectors)
            {
                if(!interfaces.ContainsKey(item.Key)) interfaces.Add(item.Key, new List<WireConnector>());
                interfaces[item.Key].Add(item.Value);
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
