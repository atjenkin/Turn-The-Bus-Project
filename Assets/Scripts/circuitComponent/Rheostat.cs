using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rheostat : CircuitComponent
{
    private event EventHandler OnComponentChanged;
    private RheostatSlider slider;
    public float Ratio = 0.5f;
    public double Resistance;
    public const double MinResistance = 1.0e-6;

    protected override void Start() 
    {
        base.Start();
        slider = gameObject.GetComponentInChildren<RheostatSlider>();
    }

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();

        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name+"_RLeft", interfaces[0], interfaces[1], parameters[0]/2));
        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name+"_RRight", interfaces[1], interfaces[2], parameters[0]/2));
        Resistance = (double)parameters[0];
    }

    public override void RegisterComponent(Circuit circuit) 
    {
        base.RegisterComponent(circuit);

        OnComponentChanged += (sender, args) => 
        {
            circuit.RunCircuit();
        };
    }

    void Update() 
    {
        float sliderRatio = slider.Ratio;
        if(Ratio != sliderRatio)
        {
            spiceEntitys[0].SetParameter<double>("resistance", Math.Max((double)sliderRatio * Resistance, MinResistance));
            spiceEntitys[1].SetParameter<double>("resistance", Math.Max((1-(double)sliderRatio) * Resistance, MinResistance));
            if(OnComponentChanged != null) {
                OnComponentChanged(this, new EventArgs());
            }
        }
        Ratio = sliderRatio;
    }
}
