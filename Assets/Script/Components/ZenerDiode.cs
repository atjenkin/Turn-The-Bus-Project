 /*using SpiceSharp.Attributes;
using SpiceSharp.Components;
using SpiceSharp.Entities;
using SpiceSharp.ParameterSets;
using System.Collections.Generic;
using UnityEngine;

internal class DiodeModelParameters : ParameterSet
{
    [ParameterName("bv"), ParameterInfo("Breakdown voltage")]
    public double BreakdownVoltage { get; set; }

    [ParameterName("is"), ParameterInfo("Reverse saturation current")]
    public double ReverseSaturationCurrent { get; set; }
    *********
}*/
using UnityEngine;
using SpiceSharp.Components;
using SpiceSharp.Entities;
using System.Collections;
using System.Collections.Generic;
using SpiceSharp.Components.Diodes;

public class ZenerDiode : CircuitComponent
{
    public const string MATERIAL_PATH = "Materials/Diode materials/";
    private static int diodeCounter = 0;

    public float BreakdownVoltage = 10.0f; // Default breakdown voltage
    public float ReverseSaturationCurrent = 1e-14f; // Default saturation current
    public string AnodeNode = "anode";
    public string CathodeNode = "cathode";
    public string model;
    public Material cathode;
    public Material anode;

    public GameObject Anode;
    public GameObject Cathode;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

        // Validate parameters length
        if (parameters != null && parameters.Length >= 2)
        {
            BreakdownVoltage = parameters[0];
            ReverseSaturationCurrent = parameters[1];
        }
        else
        {
            Debug.LogWarning("Insufficient parameters provided. Using default values.");
        }

        // Initialize spiceEntitys list
        if (spiceEntitys == null)
        {
            spiceEntitys = new List<IEntity>();
        }
        string uniqueModelName = $"dz_{diodeCounter++}";


        // Create the diode model with the name "dz"
        var zenerModel = new DiodeModel(uniqueModelName);
        zenerModel.SetParameter("bv", (double)BreakdownVoltage);
        zenerModel.SetParameter("is", (double)ReverseSaturationCurrent);

        // Add the model to the entities list
        spiceEntitys.Add(zenerModel);

        // Set the model name
        model = uniqueModelName;
        spiceEntitys.Add(new SpiceSharp.Components.Diode(name, interfaces[0], interfaces[1], model));
    }

    protected override void Start()
    {
        base.Start();

        // Check if Anode and Cathode GameObjects are assigned
        if (Anode != null && Cathode != null)
        {
            // Set materials
            /*Anode.GetComponent<Renderer>().material = anode;
            Cathode.GetComponent<Renderer>().material = cathode;*/
            Renderer anodeRenderer = Anode.GetComponent<Renderer>();
            Renderer cathodeRenderer = Cathode.GetComponent<Renderer>();

            if (anodeRenderer != null && cathodeRenderer != null)
            {
                // Set materials
                anodeRenderer.material = anode;
                cathodeRenderer.material = cathode;
            }
            else
            {
                Debug.LogError("Renderer component missing on Anode or Cathode GameObject!");
            }
        }
        else
        {
            Debug.LogError("Anode or Cathode GameObjects not assigned!");
        }
    }

    void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
    }
}
