using UnityEngine;
using SpiceSharp.Components;
using SpiceSharp.Entities;
using System.Collections.Generic;

public class BJTransistor : CircuitComponent
{
    public const string MATERIAL_PATH = "Materials/Transistormaterials/";
    private static int transistorCounter = 0;

    public float SaturationCurrent = 1e-14f; // Default saturation current
    public float ForwardBeta = 100f; // Default forward beta
    public float ReverseBeta = 1f; // Default reverse beta
    public string CollectorNode = "collector";
    public string BaseNode = "base";
    public string EmitterNode = "emitter";
    public string sub = "substrate";
    public string model;
    public Material collectorMaterial;
    public Material baseMaterial;
    public Material emitterMaterial;

    public GameObject Collector;
    public GameObject Base;
    public GameObject Emitter;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

        // Validate parameters length
        if (parameters != null && parameters.Length >= 3)
        {
            SaturationCurrent = parameters[0];
            ForwardBeta = parameters[1];
            ReverseBeta = parameters[2];
        }

        // Initialize spiceEntitys list
        spiceEntitys = new List<IEntity>();
        string uniqueModelName = $"npn_{transistorCounter++}";

        // Create the transistor model with the name "npn"
        var npnModel = new BipolarJunctionTransistorModel(uniqueModelName);
        npnModel.SetParameter("is", (double)SaturationCurrent);
        npnModel.SetParameter("bf", (double)ForwardBeta);
        npnModel.SetParameter("br", (double)ReverseBeta);

        // Add the model to the entities list
        spiceEntitys.Add(npnModel);

        // Set the model name
        model = uniqueModelName;
        spiceEntitys.Add(new BipolarJunctionTransistor(name, interfaces[0], interfaces[1], interfaces[2], interfaces[3], model));
    }

    protected override void Start()
    {
        base.Start();

        // Check if Collector, Base, and Emitter GameObjects are assigned
        if (Collector != null && Base != null && Emitter != null)
        {
            // Set materials
            Collector.GetComponent<Renderer>().material = collectorMaterial;
            Base.GetComponent<Renderer>().material = baseMaterial;
            Emitter.GetComponent<Renderer>().material = emitterMaterial;
        }
        else
        {
            Debug.LogError("Collector, Base, or Emitter GameObjects not assigned!");
        }
    }

    void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
    }
}
