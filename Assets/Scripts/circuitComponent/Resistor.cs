using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Resistor : CircuitComponent
{
    public static Dictionary<int, string> MULTIPLIER_BAND_COLORS = new Dictionary<int, string>
    {
        {0, "Black"},
        {1, "Brown"},
        {2, "Red"},
        {3, "Orange"},
        {4, "Yellow"},
        {5, "Green"},
        {6, "Blud"},
        {7, "Violet"},
        {8, "Grey"},
        {9, "White"},
        {-1, "Gold"},
        {-2, "Silver"},
    };
    public static Dictionary<int, string> RESISTANCE_BAND_COLORS = new Dictionary<int, string>
    {
        {0, "Black"},
        {1, "Brown"},
        {2, "Red"},
        {3, "Orange"},
        {4, "Yellow"},
        {5, "Green"},
        {6, "Blud"},
        {7, "Violet"},
        {8, "Grey"},
        {9, "White"}
    };

    public int Multiplier;
    public int ResistorParam1;
    public int ResistorParam2;
    public double Tolerance; // tolerance not supported yet
    public double Resistance;

    public const int MAX_MULTIPLIER = 6;
    public const int MIN_MULTIPLIER = -2;
    public const int MAX_RESISTANCE_PARAM = 9;
    public const int MIN_RESISTANCE_PARAM = 0;
    public const string MATERIAL_PATH = "Materials/Resistor Materials/";

    public GameObject Band1;
    public GameObject Band2;
    public GameObject BandMultiplier;
    public GameObject BandTolerance;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;
        
        ResistorParam1 = Math.Max(Math.Min((int)parameters[0], MAX_RESISTANCE_PARAM), MIN_RESISTANCE_PARAM);
        ResistorParam2 = Math.Min(Math.Min((int)parameters[1], MAX_RESISTANCE_PARAM), MIN_RESISTANCE_PARAM);
        Multiplier     = Math.Max(Math.Min((int)parameters[2], MAX_MULTIPLIER), MIN_MULTIPLIER);
        Resistance = (ResistorParam1 * 10 + ResistorParam2) * Math.Pow(10, Multiplier);

        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], Resistance));
    }

    protected override void Start()
    {
        base.Start();
        Band1.GetComponent<Renderer>().material = loadBandColor(ResistorParam1, RESISTANCE_BAND_COLORS);
        Band2.GetComponent<Renderer>().material = loadBandColor(ResistorParam2, RESISTANCE_BAND_COLORS);
        BandMultiplier.GetComponent<Renderer>().material = loadBandColor(Multiplier, MULTIPLIER_BAND_COLORS);
    }

    private Material loadBandColor(int param, Dictionary<int, string> colorDict) 
    {
        string colorName = colorDict[param];
        return Resources.Load<Material>(MATERIAL_PATH+colorName);
    }

    void OnMouseDown(){
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
    }

}
