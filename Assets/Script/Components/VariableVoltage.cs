using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableVoltage : CircuitComponent
{
    private event EventHandler OnComponentChanged;
    public GameObject slider;
    public double Ratio = 0.5f;
    public double MaxVol=0;
    public double MinVol=0;
    public double max=0;
    public double min=0;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

        MinVol = parameters[0];
        MaxVol = parameters[1];

        min = parameters[0];
        max = parameters[1];

        if (Ratio >= 0.5) { MaxVol = 2 * parameters[1] * (Ratio - 0.5); MinVol = 0; }
        else {MinVol = 2 * -1 * parameters[0] * (0.5 - Ratio); MaxVol = 0; }

        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(new SpiceSharp.Components.VoltageSource(name+"0", interfaces[0], interfaces[1], MaxVol));
        spiceEntitys.Add(new SpiceSharp.Components.VoltageSource(name+"1", interfaces[2], interfaces[3], MinVol));
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
            if (sliderRatio > 0.5) { MaxVol = 2 * max * (sliderRatio - 0.5); MinVol = 0; }
            else {MinVol = 2 * -1 * min * (0.5 - sliderRatio); MaxVol = 0; }
            spiceEntitys[0].SetParameter<double>("dc", MaxVol);
            spiceEntitys[1].SetParameter<double>("dc", MinVol);
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
        Circuit.componentValue = string.Format("{0:0.##}", MaxVol-MinVol) + " V";
    }

}
