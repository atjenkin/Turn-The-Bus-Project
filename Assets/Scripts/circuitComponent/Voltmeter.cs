using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Voltmeter : CircuitComponent
{
    public double Indicator = 0;
    public float Scale = 1;

    private bool isLabelDisplayed = false;

    
    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;

        this.Scale = parameters[0];
        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        // A voltmeter can be treated as a resistor with extremely high resistance
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

    void OnMouseDown(){
        Debug.Log("onMouseClicked");
    }

    void Update() 
    {
        
    }
}
