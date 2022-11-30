using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlugKey : CircuitComponent
{
    public const double MaxResistance = Double.MaxValue;
    public const double MinResistance = 1.0e-6;

    public bool PlugState = false;

    private event EventHandler OnComponentChanged;
    private SwitchButton button;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

        // A Plug key can be treated as a resistor with infinite resistance
        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], MaxResistance));
    }

    public override void RegisterComponent(Circuit circuit) 
    {
        base.RegisterComponent(circuit);

        OnComponentChanged += (sender, args) => 
        {
            circuit.RunCircuit();
        };
    }
    
    protected override void Start() 
    {
        base.Start();
        button = gameObject.GetComponentInChildren<SwitchButton>();
    }

    void Update() 
    {
        bool plugIn = button.PlugIn;
        if(!plugIn && PlugState)
        {
            spiceEntitys[0].SetParameter<double>("resistance", MaxResistance);
            if(OnComponentChanged != null) {
                OnComponentChanged(this, new EventArgs());
            }
        }
        else if(plugIn && !PlugState)
        {
            spiceEntitys[0].SetParameter<double>("resistance", MinResistance);
            if(OnComponentChanged != null) {
                OnComponentChanged(this, new EventArgs());
            }
        }
        PlugState = plugIn;
    }
}
