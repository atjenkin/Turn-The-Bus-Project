using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ammeter : CircuitComponent
{
    public double Indicator = 0;
    public float Scale = 1.0e3f;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;

        this.Scale = parameters[0];
        // A ammeter can be treated as a resistor with extremely low resistance
        spiceEntity = new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], parameters[1]);
    }

    public override void RegisterComponent(Circuit circuit) 
    {
        var currentExport = new SpiceSharp.Simulations.RealPropertyExport(circuit.Sim, this.Name, "i");
        circuit.Sim.ExportSimulationData += (sender, args) =>
        {
            this.Indicator = currentExport.Value;
            Debug.Log(string.Format("Ammeter: {0:0.##}", this.Indicator * this.Scale));
        };
    }

    void Update() 
    {
    }
}
