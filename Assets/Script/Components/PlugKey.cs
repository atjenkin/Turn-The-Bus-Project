using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugKey : CircuitComponent
{
    public const double MaxResistance = double.MaxValue;
    public const double MinResistance = 0;

    public bool PlugState = false;

    private event EventHandler OnComponentChanged;
    public GameObject button;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

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

    protected override void Update()
    {
        base.Update();

        bool plugIn = button.GetComponent<SwitchButton>().PlugIn;
        if (!plugIn && PlugState)
        {
            spiceEntitys[0].SetParameter<double>("resistance", MaxResistance);
            Circuit.isLabelWindowOpen = true;
            Circuit.componentTitle = Title;
            Circuit.componentDescription = Description;
            Circuit.componentValue = "OFF";
            if (OnComponentChanged != null)
            {
                OnComponentChanged(this, new EventArgs());
            }
        }
        else if (plugIn && !PlugState)
        {
            spiceEntitys[0].SetParameter<double>("resistance", MinResistance);
            Circuit.isLabelWindowOpen = true;
            Circuit.componentTitle = Title;
            Circuit.componentDescription = Description;
            Circuit.componentValue = "ON";
            if (OnComponentChanged != null)
            {
                OnComponentChanged(this, new EventArgs());
            }
        }
        PlugState = plugIn;
    }

    private void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
        if (PlugState) { Circuit.componentValue = "ON"; }
        else { Circuit.componentValue = "OFF";  }
    }
}
