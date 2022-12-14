/*
* This file was developed by a team from Carnegie Mellon University as a part of the practicum project for Fall 2022 in collaboration with Turn The Bus.
* Authors: Adrian Jenkins, Harshit Maheshwari, and Ziniu Wan. (Carnegie Mellon University)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Ammeter : CircuitComponent
{
    public double Indicator = 0;
    public float Scale = 1.0e3f;

    public string componentTitleString = "";
    public string componentDescriptionString = "";


    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Scale = parameters[0];
        this.Title = title;
        this.Description = description;

        // An ammeter can be treated as a resistor with extremely low resistance
        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], parameters[1]));
    }

    public override void RegisterComponent(Circuit circuit) 
    {
        base.RegisterComponent(circuit);

        gameObject.GetComponentInChildren<AmmeterText>().InitAmmeterValue();
        var currentExport = new SpiceSharp.Simulations.RealPropertyExport(circuit.Sim, this.Name, "i");
        circuit.Sim.ExportSimulationData += (sender, args) =>
        {
            this.Indicator = currentExport.Value;
            
            gameObject.GetComponentInChildren<AmmeterText>().UpdateAmmeterValue(this.Indicator * this.Scale);
        };
    }

    void OnMouseDown(){
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
        
    }

}
