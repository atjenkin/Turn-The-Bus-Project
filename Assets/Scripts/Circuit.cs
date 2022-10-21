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
    public TextAsset textJSON;

    public List<CircuitComponent> circuitComponents;

    private SpiceSharp.Circuit ckt;
    private SpiceSharp.Simulations.BiasingSimulation simulation;

    public ComponentMetaList componentMetaList = new ComponentMetaList();
    

    /**************** Methods ****************/
    // Start is called before the first frame update
    void Start()
    {
        circuitComponents = new List<CircuitComponent>();
        componentMetaList = JsonUtility.FromJson<ComponentMetaList>(textJSON.text);

        initCircuit();
        simulation.Run(ckt);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void initCircuit() 
    {
        ckt = new SpiceSharp.Circuit();
        simulation = new SpiceSharp.Simulations.OP("Sim");
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
            ckt.Add(thisComponent.GetSpiceEntity());
            thisComponent.RegisterSimulation(simulation, ckt);
        }
    }
}
