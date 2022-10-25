using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlugKey : CircuitComponent
{
    public const double MaxResistance = Double.MaxValue;
    public const double MinResistance = 1.0e-6;

    public bool PlugIn = false;

    private event EventHandler OnComponentChanged;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;

        // A Plug key can be treated as a resistor with infinite resistance
        spiceEntity = new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], MaxResistance);
    }

    public override void RegisterComponent(Circuit circuit) 
    {
        OnComponentChanged += (sender, args) => 
        {
            circuit.RunCircuit();
        };
    }
    
    void OnMouseDown() {
        plugSwitch();

        if(OnComponentChanged != null) {
            OnComponentChanged(this, new EventArgs());
        }
    }

    private void plugSwitch()
    {
        if(PlugIn) 
        {
            PlugIn = false;
            spiceEntity.SetParameter<double>("resistance", MaxResistance);
            Debug.Log("Plug key plugged out!");
        }
        else 
        {
            PlugIn = true;
            spiceEntity.SetParameter<double>("resistance", MinResistance);
            Debug.Log("Plug key plugged in!");
        }
    }
}
