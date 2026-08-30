using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CurrentMeasure : CircuitComponent
{
    [SerializeField] TextMeshPro Label;

    public double Indicator = 0;
    public float Scale = 1.0e3f;

    public string componentTitleString = "";
    public string componentDescriptionString = "";

    double leastCount = 0f;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Scale = parameters[0];
        this.Title = title;
        this.Description = description;

        leastCount = parameters[2];

        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], parameters[1]));

        Label.text = Title;
    }

    public override void RegisterComponent(Circuit circuit)
    {
        base.RegisterComponent(circuit);

        gameObject.GetComponentInChildren<CurrentMeasureText>().InitAmmeterValue();
        var currentExport = new SpiceSharp.Simulations.RealPropertyExport(circuit.Sim, this.name, "i");
        circuit.Sim.ExportSimulationData += (sender, args) =>
        {
            this.Indicator = currentExport.Value;
            gameObject.GetComponentInChildren<CurrentMeasureText>().UpdateAmmeterValue(this.Indicator * this.Scale, leastCount);
        };
    }

    private void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;

        double currentValue = (this.Indicator * this.Scale)/1000;

        if (Math.Abs(currentValue) >= leastCount)
        {
            double adjustedValue = Math.Floor(currentValue / leastCount) * leastCount;
            Circuit.componentValue = string.Format("{0:0.##}", adjustedValue) + " A";
        }
        else
        {
            Circuit.componentValue = "0 A";
        }

        Circuit.componentValue += " , Least Count : " + string.Format("{0:0.###}", leastCount);

    }
}
