using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltageSource : CircuitComponent
{
    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;

        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(new SpiceSharp.Components.VoltageSource(name, interfaces[0], interfaces[1], parameters[0]));
    }

    void OnMouseDown(){
        Circuit.isLabelWindowOpen = true;
        Debug.Log("onMouseClicked: Voltage Source");
    }
}
