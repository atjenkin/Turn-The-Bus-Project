using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using WireBuilder;

public class Circuit : MonoBehaviour
{
    /*************** JSON DEFINING ***************/
    [System.Serializable] public class ComponentMeta
    {
        public string Name;
        public string Type;
        public string Title;
        public string Description;
        public float[] Position;
        public float[] Rotation;
        public float[] Scale;
        public string[] Interfaces;
        public float[] Parameters;
    }

    [System.Serializable] public class ComponentMetaList
    {
        public ComponentMeta[] Components;
    }

    [System.Serializable] public class ExperimentInfo
    {
        public string ExperimentTitle;
        public string Aim;
        public string Background;
        public string Diagram;
    }

    [System.Serializable] public class ExpMaterials
    {
        public string[] MaterialRequired;
    }

    [System.Serializable] public class LabProcedure
    {
        public string[] Procedure;
    }

    [System.Serializable] public class ExpObservations
    {
        public string[] Observations;
    }

    /*************** Components ***************/
    public static string expJSON;

    public TextMeshProUGUI ExpTitleField;
    public TextMeshProUGUI componentTitleField;
    public TextMeshProUGUI componentDescriptionField;
    public TextMeshProUGUI componentValueField;

    public static string componentTitle = "";
    public static string componentDescription = "";
    public static string componentValue = "";
    public List<CircuitComponent> circuitComponents;

    public SpiceSharp.Circuit Ckt;
    public SpiceSharp.Simulations.BiasingSimulation Sim;

    public ComponentMetaList componentMetaList = new ComponentMetaList();

    public const string PREFAB_PATH = "PreFabs/Lab/Components/";
    public static bool isLabelWindowOpen = false;

    private List<Wire> wires = new List<Wire>();

    /*************** FUNCTIONS ***************/
    // Start is called before the first frame update
    void Start()
    {
        TextAsset textJSON = Resources.Load<TextAsset>(expJSON);
        circuitComponents = new List<CircuitComponent>();
        componentMetaList = JsonUtility.FromJson<ComponentMetaList>(textJSON.text);
        Debug.Log(componentMetaList.ToString());
        InitUIWidgets(textJSON);
        InitCircuit();
        RunCircuit();
    }

    // Update is called once per frame
    void Update()
    {
        updateLabelInfo();
    }

    public void InitUIWidgets(TextAsset textJSON)
    {
        ExperimentInfo expInfoJSON = JsonUtility.FromJson<ExperimentInfo>(textJSON.text);
        ExpTitleField.SetText(expInfoJSON.ExperimentTitle);
    }

    public void updateLabelInfo()
    {
        if(isLabelWindowOpen)
        {
            componentTitleField.SetText(componentTitle);
            componentDescriptionField.SetText(componentDescription);
            componentValueField.SetText(componentValue);
            componentTitleField.gameObject.SetActive(true);
            componentDescriptionField.gameObject.SetActive(true);
            componentValueField.gameObject.SetActive(true);
        }
        else
        {
            componentTitleField.gameObject.SetActive(false);
            componentDescriptionField.gameObject.SetActive(false);
            componentValueField.gameObject.SetActive(false);
        }
    }

    public void InitCircuit()
{
    Ckt = new SpiceSharp.Circuit();
    Sim = new SpiceSharp.Simulations.OP("Sim");

    if (componentMetaList?.Components == null)
    {
        Debug.LogError("ComponentMetaList or its Components array is null!");
        return;
    }

    foreach (ComponentMeta meta in componentMetaList.Components)
    {
        if (meta == null)
        {
            Debug.LogError("ComponentMeta is null!");
            continue;
        }

        string prefabPath = PREFAB_PATH + meta.Type;
        Debug.Log("Loading prefab from path: " + prefabPath);

        GameObject prefabObject = Resources.Load<GameObject>(prefabPath);

        if (prefabObject == null)
        {
            Debug.LogError("Prefab not found at path: " + prefabPath);
            continue;
        }

        Debug.Log("Prefab loaded successfully: " + prefabObject.name);
        GameObject instance = Instantiate(prefabObject, this.transform, true);
        instance.name = meta.Name;

        if (meta.Position != null)
        {
            instance.transform.position = new Vector3(meta.Position[0], meta.Position[1], meta.Position[2]);
        }
        else
        {
            Debug.LogError("Position array is null for component: " + meta.Name);
        }

        if (meta.Rotation != null)
        {
            instance.transform.Rotate(meta.Rotation[0], meta.Rotation[1], meta.Rotation[2], Space.World);
        }
        else
        {
            Debug.LogError("Rotation array is null for component: " + meta.Name);
        }

        if (meta.Scale != null)
        {
            instance.transform.localScale = new Vector3(meta.Scale[0], meta.Scale[1], meta.Scale[2]);
        }
        else
        {
            Debug.LogError("Scale array is null for component: " + meta.Name);
        }

        CircuitComponent thisComponent = instance.GetComponent<CircuitComponent>();
        if (thisComponent != null)
        {
            thisComponent.InitSpiceEntity(meta.Name, meta.Interfaces, meta.Parameters, meta.Title, meta.Description);
            circuitComponents.Add(thisComponent);
            thisComponent.RegisterComponent(this);
            thisComponent.InitInterfaces(meta.Interfaces);
        }
        else
        {
            Debug.LogError("CircuitComponent not found on instantiated prefab: " + instance.name);
        }
    }
}



    public void GenerateWires()
    {
        Dictionary<string, List<WireConnector>> interfaces = new Dictionary<string, List<WireConnector>>();
        foreach (CircuitComponent thisComponent in circuitComponents)
        {
            for (int i = 0; i < thisComponent.connectors.Count; i++)
            {
                string interfaceName = thisComponent.Interfaces[i];
                WireConnector connector = thisComponent.connectors[i];
                if (!interfaces.ContainsKey(interfaceName)) interfaces.Add(interfaceName, new List<WireConnector>());
                interfaces[interfaceName].Add(connector);
            }
        }
        foreach (var item in interfaces)
        {
            for (int i = 1; i < item.Value.Count; i++)
            {
                Wire wire = WireManager.CreateWireObject(item.Value[i - 1], item.Value[i], item.Value[i].wireType);
                wire.transform.SetParent(this.transform);
                wire.wireType.diameter = 0.04f;
                wires.Add(wire);
            }
        }
    }

    public void DestroyWires()
    {
        foreach (Wire wire in wires)
        {
            WireManager.DestroyWire(wire);
        }
        wires = new List<Wire>();
    }

    public void RunCircuit()
    {
        Sim.Run(Ckt);
    }
}
