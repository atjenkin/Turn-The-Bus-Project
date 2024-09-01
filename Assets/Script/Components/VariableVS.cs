using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VariableVS : CircuitComponent
{
    private event EventHandler OnComponentChanged;
    public GameObject slider;
    public double Ratio = 0.5f;
    public TextMeshPro VoltageText;
    private double voltage;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(new SpiceSharp.Components.VoltageSource(name + "_RLeft", interfaces[0], interfaces[1], parameters[1] * Ratio));
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
            voltage = Math.Max(sliderRatio * this.Parameters[1], this.Parameters[0]);
            spiceEntitys[0].SetParameter("dc", voltage);
            if (OnComponentChanged != null)
            {
                OnComponentChanged(this, new EventArgs());
            }
            UpdateVoltageText();
        }
        Ratio = sliderRatio;
        
    }
    private void UpdateVoltageText()
    {
        VoltageText.text = "Voltage: " + voltage.ToString("F2") + "V";
    }
}
