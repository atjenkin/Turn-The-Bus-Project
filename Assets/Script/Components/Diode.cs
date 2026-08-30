using SpiceSharp.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diode : CircuitComponent
{
    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

        // Defaults modeled after a general 1N4007 diode.
        double saturationCurrent = 7.69e-11;
        double seriesResistance = 4.2e-2;
        double breakdownVoltage = 1.0e3;
        double breakdownCurrent = 5.0e-6;
        double emissionCoefficient = 1.45e0;

        // Optional overrides from experiment JSON: [bv, ibv, is, n, rs]
        if (parameters != null)
        {
            if (parameters.Length >= 1) { breakdownVoltage = parameters[0]; }
            if (parameters.Length >= 2) { breakdownCurrent = parameters[1]; }
            if (parameters.Length >= 3) { saturationCurrent = parameters[2]; }
            if (parameters.Length >= 4) { emissionCoefficient = parameters[3]; }
            if (parameters.Length >= 5) { seriesResistance = parameters[4]; }
        }

        string modelName = $"{name}_model";
        var model = new DiodeModel(modelName);
        model.SetParameter("is", saturationCurrent);//Saturation current
        model.SetParameter("rs", seriesResistance);//Series Resistance
        model.SetParameter("bv", breakdownVoltage);//Reverse Breakdown voltage
        model.SetParameter("ibv", breakdownCurrent);//Reverse Breakdown Current
        model.SetParameter("cjo", 2.65e-11);//Zero-bias junction capacitance
        model.SetParameter("m", 3.33e-1);//Grading coefficient
        model.SetParameter("n", emissionCoefficient);//Emission coefficient
        model.SetParameter("tt", 4.32e-6);//transi-time

        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(model);
        spiceEntitys.Add(new SpiceSharp.Components.Diode(name, interfaces[0], interfaces[1], modelName));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
        Circuit.componentValue = "1N4007 DIODE";
    }
}
