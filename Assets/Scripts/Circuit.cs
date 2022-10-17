using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Circuit : MonoBehaviour
{
    public Battery[] batteries;
    public Wire[] wires;
    public Resistor[] resistors;

    private SpiceSharp.Circuit ckt;
    private SpiceSharp.Simulations.DC dc;

    // Start is called before the first frame update
    void Start()
    {
        generateComponents();

        batteries = GetComponentsInChildren<Battery>();
        wires = GetComponentsInChildren<Wire>();
        resistors = GetComponentsInChildren<Resistor>();

        initCircuit();
        runCircuit();
    }

    private void generateComponents() 
    {
        // Hardcode for now
        Dictionary<string, int> numPrefabs = new Dictionary<string, int>();
        numPrefabs.Add("Assets/Prefabs/Resistor.prefab", 2);
        //numPrefabs.Add("Assets/Prefabs/Wire.prefab", 3);
        numPrefabs.Add("Assets/Prefabs/Battery.prefab", 1);

        Dictionary<string, List<string>> namePrefabs = new Dictionary<string, List<string>>();
        namePrefabs.Add("Assets/Prefabs/Resistor.prefab", new List<string>(){"Resistor1", "Resistor2"});
        //namePrefabs.Add("Assets/Prefabs/Wire.prefab", new List<string>(){"Wire1", "Wire2", "Wire3"});
        namePrefabs.Add("Assets/Prefabs/Battery.prefab", new List<string>(){"Battery1"});

        Dictionary<string, List<Vector3>> posPrefabs = new Dictionary<string, List<Vector3>>();
        posPrefabs.Add("Assets/Prefabs/Resistor.prefab", new List<Vector3>(){new Vector3(2.18f, 2.3f, -0.204f), new Vector3(2.18f, 2.3f, -3f)});
        //posPrefabs.Add("Assets/Prefabs/Wire.prefab", new List<Vector3>(){new Vector3(), new Vector3(), new Vector3()});
        posPrefabs.Add("Assets/Prefabs/Battery.prefab", new List<Vector3>(){new Vector3(0.5f, 2.3f, -1.5f)});

        //Dictionary<string, >

        string[] prefabGUIDs = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs"});
        foreach(string guid in prefabGUIDs)
        { 
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            if(!numPrefabs.ContainsKey(prefabPath)) {
                continue;
            }
            GameObject prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            for(int i=0; i<numPrefabs[prefabPath]; i++) {
                var instance = Instantiate(prefabObject, this.transform, true);
                instance.name = namePrefabs[prefabPath][i];
                instance.transform.position = posPrefabs[prefabPath][i];
            }
        }
    }

    private void initCircuit() 
    {
        // dummy circuit initialization
        batteries[0].voltageSource = new SpiceSharp.Components.VoltageSource("V1", "in", "0", 3.0);
        resistors[0].resistor = new SpiceSharp.Components.Resistor("R1", "in", "out", 1.0e4);
        resistors[1].resistor = new SpiceSharp.Components.Resistor("R2", "out", "0", 2.0e4);
        ckt = new SpiceSharp.Circuit(
            batteries[0].voltageSource,
            resistors[0].resistor,
            resistors[1].resistor
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
            Debug.Log(string.Format("voltage: {0:0.##}; current: {1:0.####}", voltageExport.Value, currentExport.Value));
        };
        dc.Run(ckt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
