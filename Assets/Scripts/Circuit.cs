using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Circuit : MonoBehaviour
{
    public TextAsset textJSON;

    public List<CircuitComponent> circuitComponents;

    private SpiceSharp.Circuit ckt;
    private SpiceSharp.Simulations.DC dc;
    
    [System.Serializable]
    public class ComponentMeta
    {
        public string name;
        public string type;
        public float[] position;
        public string[] interfaces;
        public float[] parameters;
    }

    [System.Serializable]
    public class ComponentMetaList
    {
        public ComponentMeta[] Components;
    }

    public ComponentMetaList componentMetaList = new ComponentMetaList();

    // Start is called before the first frame update
    void Start()
    {
        circuitComponents = new List<CircuitComponent>();
        componentMetaList = JsonUtility.FromJson<ComponentMetaList>(textJSON.text);

        initCircuit();
        runCircuit();
    }

    private void initCircuit() 
    {
        foreach(ComponentMeta meta in componentMetaList.Components) 
        {
            string guid = AssetDatabase.FindAssets(meta.type, new string[] {"Assets/Prefabs"})[0];
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            var instance = Instantiate(prefabObject, this.transform, true);
            instance.name = meta.name;
            instance.transform.position = new Vector3(meta.position[0], meta.position[1], meta.position[2]);

            CircuitComponent thisComponent = instance.GetComponent<CircuitComponent>();
            thisComponent.InitSpiceEntity(meta.name, meta.interfaces, meta.parameters);
            circuitComponents.Add(thisComponent);
        }
        ckt = new SpiceSharp.Circuit(
            ((IEnumerable<CircuitComponent>)circuitComponents).Select(c => c.GetSpiceEntity())
        );
    }

    private void runCircuit()
    {
        // dummy simulation
        dc = new SpiceSharp.Simulations.DC("Sim", "V1", 3.0, 3.0, 0.1);
        var voltageExport = new SpiceSharp.Simulations.RealVoltageExport(dc, "out");
        var currentExport = new SpiceSharp.Simulations.RealPropertyExport(dc, "R1", "i");
        dc.ExportSimulationData += (sender, args) =>
        {
            Debug.Log(string.Format("voltage on output node: {0:0.##}; current through R1: {1:0.####}", voltageExport.Value, currentExport.Value));
        };
        dc.Run(ckt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
