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
        spiceEntity = new SpiceSharp.Components.VoltageSource(name, interfaces[0], interfaces[1], parameters[0]);
    }
}
