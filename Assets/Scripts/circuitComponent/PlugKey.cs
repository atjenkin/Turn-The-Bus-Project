using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlugKey : CircuitComponent
{
    public const double PlugOutResistance = Double.MaxValue;
    public const double PlugInResistance = 1.0e-6;

    public bool PlugIn = false;
    public delegate void CircuitEvent(object sender, EventArgs args);
    public event CircuitEvent OnCircuitChanged;


    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;

        // A klug key can be treated as a resistor with infinite resistance
        spiceEntity = new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], PlugOutResistance);
    }

    public override void RegisterComponent(Circuit circuit) 
    {
        OnCircuitChanged += (sender, args) => 
        {
            circuit.RunCircuit();
        };
    }
    
    void OnMouseDown() {
        plugSwitch();

        if(OnCircuitChanged != null) {
            OnCircuitChanged(this, new EventArgs());
        }
    }

    private void plugSwitch()
    {
        if(PlugIn) 
        {
            PlugIn = false;
            spiceEntity.SetParameter<double>("resistance", PlugOutResistance);
            Debug.Log("Plug key plugged out!");
        }
        else 
        {
            PlugIn = true;
            spiceEntity.SetParameter<double>("resistance", PlugInResistance);
            Debug.Log("Plug key plugged in!");
        }
    }
}
