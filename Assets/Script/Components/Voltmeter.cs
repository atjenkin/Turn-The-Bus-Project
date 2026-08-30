using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voltmeter : CircuitComponent
{
    public double Indicator = 0;
    public float Scale = 1;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

        this.Scale = parameters[0];
        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], parameters[1]));
    }

    public override void RegisterComponent(Circuit circuit)
    {
        base.RegisterComponent(circuit);

        gameObject.GetComponentInChildren<VoltmeterText>().InitVoltageValue();
        var voltageExport = new SpiceSharp.Simulations.RealVoltageExport(circuit.Sim, this.Interfaces[0], this.Interfaces[1]);
        circuit.Sim.ExportSimulationData += (sender, args) =>
        {
            this.Indicator = voltageExport.Value;

            gameObject.GetComponentInChildren<VoltmeterText>().UpdateVoltageValue(this.Indicator * this.Scale);
        };
    }

    private void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;

        double scaledIndicator = Indicator * Scale;
        string displayValue;

        //if (scaledIndicator >= 100 || scaledIndicator <= -100) { displayValue = string.Format("{0:0.##}", (scaledIndicator / 1000)) + " V"; }
        //else { displayValue = string.Format("{0:0.##}", scaledIndicator) + " mV"; }
        displayValue = string.Format("{0:0.##}", scaledIndicator) + " V";
        Circuit.componentValue = displayValue;
    }
}
