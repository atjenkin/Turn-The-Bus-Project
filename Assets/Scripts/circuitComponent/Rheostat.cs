using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rheostat : CircuitComponent
{
    private event EventHandler OnComponentChanged;
    private RheostatSlider slider;
    public double Ratio = 0.5f;
    public double MaxResistance;
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

        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name+"_RLeft", interfaces[0], interfaces[1], parameters[0]*Ratio));
        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name+"_RRight", interfaces[1], interfaces[2], parameters[0]*(1-Ratio)));
        MaxResistance = (double)parameters[0];
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
        double sliderRatio = slider.Ratio;
        if(Ratio != sliderRatio && spiceEntitys!=null)
        {
            spiceEntitys[0].SetParameter<double>("resistance", Math.Max(sliderRatio * MaxResistance, MinResistance));
            spiceEntitys[1].SetParameter<double>("resistance", Math.Max((1-sliderRatio) * MaxResistance, MinResistance));
            if(OnComponentChanged != null) {
                OnComponentChanged(this, new EventArgs());
            }
        }
        Ratio = sliderRatio;
    }

    void OnMouseDown(){
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = "Rheostat";
        Circuit.componentDescription = "Rheostat description";
        
    }
}
