using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

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
            Ckt.Add(thisComponent.GetSpiceEntity());
            thisComponent.RegisterComponent(this);
        }
    }

    public void RunCircuit()
    {
        Sim.Run(Ckt);
    }
}
