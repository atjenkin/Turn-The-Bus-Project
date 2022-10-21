using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Voltmeter : CircuitComponent
{
    public double Indicator = 0;
    public float Scale = 1;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;

        this.Scale = parameters[0];
        // A ideal voltmeter can be treated as a resistor with infinite resistance in a circuit
        spiceEntity = new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], Double.MaxValue);
    }

    public override void RegisterSimulation(SpiceSharp.Simulations.IBiasingSimulation sim) 
    {
        var voltageExport = new SpiceSharp.Simulations.RealVoltageExport(sim, this.Interfaces[0], this.Interfaces[1]);
        sim.ExportSimulationData += (sender, args) =>
        {
            this.Indicator = voltageExport.Value;
        };
    }

    void Update() 
    {
        Debug.Log(string.Format("Voltage shown on the Voltmeter: {0:0.##}", this.Indicator * this.Scale));
    }
}
