using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using SpiceSharp.Components.Diodes;
using SpiceSharp.Attributes;
using SpiceSharp.ParameterSets;



public class Zener_Diode : CircuitComponent
{
    public const string MATERIAL_PATH = "Materials/Diode materials/";

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


        if (parameters.Length >= 1)
            BreakdownVoltage = parameters[0];
        if (parameters.Length >= 2)
            ReverseSaturationCurrent = parameters[1];



        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();


        // Create the diode model with the name "dz"
        var zenerModel = new DiodeModel("dz");
        zenerModel.SetParameter("bv", (double)BreakdownVoltage);
        zenerModel.SetParameter("is", (double)ReverseSaturationCurrent);

        // Add the model to the entities list
        spiceEntitys.Add(zenerModel);

        // Set the model name
        model = "dz";
        spiceEntitys.Add(new SpiceSharp.Components.Diode(name, interfaces[0], interfaces[1], model));
    }

    protected override void Start()
    {
        base.Start();
        Anode.GetComponent<Renderer>().material = anode;
        Cathode.GetComponent<Renderer>().material = cathode;
    }

    /*private Material loadBandColor(int param, Dictionary<int, string> colorDict) 
    {
        string colorName = colorDict[param];
        return Resources.Load<Material>(MATERIAL_PATH+colorName);
    }*/

    void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
    }

    internal class DiodeModelParameters : ParameterSet
    {
        [ParameterName("bv"), ParameterInfo("Breakdown voltage")]
        public double BreakdownVoltage { get; set; }

        [ParameterName("is"), ParameterInfo("Reverse saturation current")]
        public double ReverseSaturationCurrent { get; set; }
    }
}
