using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistor : CircuitComponent
{
    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters)
    {
        spiceEntity = new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], parameters[0]);
    }
}
