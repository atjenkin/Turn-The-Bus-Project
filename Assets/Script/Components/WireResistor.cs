using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WireResistor : CircuitComponent
{
    public double areaOfCrossSection;
    public double resistivity;
    public double wireLength;
    public double Resistance;

    

    public const string MATERIAL_PATH = "Assets/Resources/Materials/Resistor Materials";

    public void InitWireResistor(double areaOfCrossSection, double resistivity, double length)
    {
        this.areaOfCrossSection = areaOfCrossSection;
        this.resistivity = resistivity;
        this.wireLength = length;
        ComputeResistance();
    }

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

        InitWireResistor(parameters[0], parameters[1], parameters[2]);
        
        spiceEntitys = new List<SpiceSharp.Entities.IEntity> ();
        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], Resistance));
    }

    private void ComputeResistance()
    {
        this.Resistance = (this.resistivity * this.wireLength) / this.areaOfCrossSection;
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
        Circuit.componentValue = string.Format("{0:0.##}", Resistance) + " OHM"+", "+ string.Format("{0:0.##}", wireLength)+" M, " + areaOfCrossSection + " M^2";
    }
}
