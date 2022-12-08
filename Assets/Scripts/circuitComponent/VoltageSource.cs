/*
* This file was developed by a team from Carnegie Mellon University as a part of the practicum project for Fall 2022 in collaboration with Turn The Bus.
* Authors: Adrian Jenkins, Harshit Maheshwari, and Ziniu Wan. (Carnegie Mellon University)
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltageSource : CircuitComponent
{
    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(new SpiceSharp.Components.VoltageSource(name, interfaces[0], interfaces[1], parameters[0]));
    }

    void OnMouseDown(){
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
    }
}
