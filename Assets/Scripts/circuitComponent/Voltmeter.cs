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
        // A voltmeter can be treated as a resistor with extremely high resistance
        spiceEntity = new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], parameters[1]);
    }

    public override void RegisterComponent(Circuit circuit) 
    {
        gameObject.GetComponentInChildren<VoltmeterText>().InitVoltageValue();
        var voltageExport = new SpiceSharp.Simulations.RealVoltageExport(circuit.Sim, this.Interfaces[0], this.Interfaces[1]);
        circuit.Sim.ExportSimulationData += (sender, args) =>
        {
            this.Indicator = voltageExport.Value;

            gameObject.GetComponentInChildren<VoltmeterText>().UpdateVoltageValue(this.Indicator * this.Scale);
        };
    }

    void Update() 
    {
    }
}
