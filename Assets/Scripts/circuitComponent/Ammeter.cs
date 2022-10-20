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
        // A ideal voltmeter can be treated as a resistor with 0 resistance in a circuit
        spiceEntity = new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], 1.0e-6);
    }

    public override void RegisterSimulation(SpiceSharp.Simulations.IBiasingSimulation sim) 
    {
        var currentExport = new SpiceSharp.Simulations.RealPropertyExport(sim, this.Name, "i");
        sim.ExportSimulationData += (sender, args) =>
        {
            this.Indicator = currentExport.Value;
        };
    }

    void Update() 
    {
        Debug.Log(string.Format("Am shown on the Ammeter: {0:0.##}", this.Indicator * this.Scale));
    }
}
