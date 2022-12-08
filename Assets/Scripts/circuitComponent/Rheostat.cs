/*
* This file was developed by a team from Carnegie Mellon University as a part of the practicum project for Fall 2022 in collaboration with Turn The Bus.
* Authors: Adrian Jenkins, Harshit Maheshwari, and Ziniu Wan. (Carnegie Mellon University)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rheostat : CircuitComponent
{
    private event EventHandler OnComponentChanged;
    public GameObject slider;
    public double Ratio = 0.5f;
    public double MaxResistance;
    public const double MinResistance = 1.0e-6;


    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;
        
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

    protected override void Update() 
    {
        base.Update();
        
        double sliderRatio = slider.GetComponent<RheostatSlider>().Ratio;
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
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
    }
}
