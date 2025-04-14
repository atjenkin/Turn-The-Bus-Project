using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rheostat : CircuitComponent
{
    private event EventHandler OnComponentChanged;
    public GameObject slider;
    public double Ratio = 0.5f;
    public double MaxResistance;
    public const double MinResistance = 0;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;
        
        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();

        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name + "_RLeft", interfaces[0], interfaces[1], parameters[0] * Ratio));
        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name + "_RRight", interfaces[1], interfaces[2], parameters[0] * (1-Ratio)));
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
        if (Ratio != sliderRatio && spiceEntitys != null)
        {
            spiceEntitys[0].SetParameter<double>("resistance", Math.Max(sliderRatio * MaxResistance, MinResistance));
            spiceEntitys[1].SetParameter<double>("resistance", Math.Max((1 - sliderRatio) * MaxResistance, MinResistance));
            if (OnComponentChanged != null)
            {
                OnComponentChanged(this, new EventArgs());
            }
        }
        Ratio = sliderRatio;
    }

    private void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
        Circuit.componentValue = string.Format("{0:0.##}", (Ratio * 100)) + " Meter"+"  "+string.Format("{0:0.##}",(Ratio*MaxResistance))+" OHM"; //100M long wire
    }
}
