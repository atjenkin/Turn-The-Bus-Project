using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Circuit : MonoBehaviour
{
    public Battery[] batteries;
    public Wire[] wires;
    public Resistor[] resistors;
    private SpiceSharp.Circuit ckt;
    private SpiceSharp.Simulations.DC dc;

    // Start is called before the first frame update
    void Start()
    {
        batteries = GetComponentsInChildren<Battery>();
        wires = GetComponentsInChildren<Wire>();
        resistors = GetComponentsInChildren<Resistor>();

        initCircuit();
        runCircuit();
    }

    private void initCircuit() 
    {
        // dummy circuit initialization
        batteries[0].voltageSource = new SpiceSharp.Components.VoltageSource("V1", "in", "0", 3.0);
        resistors[0].resistor = new SpiceSharp.Components.Resistor("R1", "in", "out", 1.0e4);
        resistors[1].resistor = new SpiceSharp.Components.Resistor("R2", "out", "0", 2.0e4);
        ckt = new SpiceSharp.Circuit(
            batteries[0].voltageSource,
            resistors[0].resistor,
            resistors[1].resistor
        );
    }

    private void runCircuit()
    {
        // dummy simulation
        dc = new SpiceSharp.Simulations.DC("Sim", "V1", 3.0, 3.0, 0.1);
        var voltageExport = new SpiceSharp.Simulations.RealVoltageExport(dc, "out");
        var currentExport = new SpiceSharp.Simulations.RealPropertyExport(dc, "R1", "i");
        dc.ExportSimulationData += (sender, args) =>
        {
            Debug.Log(string.Format("voltage: {0:0.##}; current: {1:0.####}", voltageExport.Value, currentExport.Value));
        };
        dc.Run(ckt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
