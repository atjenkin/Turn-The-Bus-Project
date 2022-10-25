using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

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
        // An ammeter can be treated as a resistor with extremely low resistance
        spiceEntity = new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], parameters[1]);
    }

    public override void RegisterComponent(Circuit circuit) 
    {
        gameObject.GetComponentInChildren<AmmeterText>().InitAmmeterValue();
        var currentExport = new SpiceSharp.Simulations.RealPropertyExport(circuit.Sim, this.Name, "i");
        circuit.Sim.ExportSimulationData += (sender, args) =>
        {
            this.Indicator = currentExport.Value;
            
            gameObject.GetComponentInChildren<AmmeterText>().UpdateAmmeterValue(this.Indicator * this.Scale);
        };
    }

    void Update() 
    {
    }
}
