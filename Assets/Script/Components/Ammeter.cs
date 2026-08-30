using SpiceSharp;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Ammeter : CircuitComponent
{
    public double Indicator = 0;     // ALWAYS in Amps (A) from SpiceSharp export
    public float Scale = 1.0e3f;     // You can keep this if your AmmeterText expects it

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Scale = parameters[0];
        this.Title = title;
        this.Description = description;

        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        // Ammeter modeled as small series resistance
        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], parameters[1]));
    }

    public override void RegisterComponent(Circuit circuit)
    {
        base.RegisterComponent(circuit);

        gameObject.GetComponentInChildren<AmmeterText>().InitAmmeterValue();
        var currentExport = new SpiceSharp.Simulations.RealPropertyExport(circuit.Sim, this.name, "i");

        circuit.Sim.ExportSimulationData += (sender, args) =>
        {
            this.Indicator = currentExport.Value; // amps

            // If your AmmeterText expects "scaled" numeric only (no units),
            // keep this behavior:
            gameObject.GetComponentInChildren<AmmeterText>()
                .UpdateAmmeterValue(this.Indicator);
        };
    }

    private void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;

        double I = this.Indicator; // amps (raw)

        string display;
        double absI = Math.Abs(I);

        if (absI >= 1.0)
        {
            display = string.Format("{0:0.###} A", I);
        }
        else if (absI >= 1e-3)
        {
            display = string.Format("{0:0.###} mA", I * 1e3);
        }
        else
        {
            display = string.Format("{0:0.###} \u03bcA", I * 1e6);
        }

        Circuit.componentValue = display;
    }
}
