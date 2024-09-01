/*using SpiceSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VariableVoltageSource : CircuitComponent
{
    public GameObject slider;
    public double Ratio = 0.5f;
    public TextMeshProUGUI voltageValueText; // Reference to the UI Text
    public Vector3 sliderOffset = new Vector3(0, 1.0f, 0); // Offset for the slider position

    private float voltage;
    private event EventHandler OnComponentChanged;
    public override void RegisterComponent(Circuit circuit)
    {
        base.RegisterComponent(circuit);

        OnComponentChanged += (sender, args) =>
        {
            circuit.RunCircuit();
        };
    }
    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;


        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(new SpiceSharp.Components.VoltageSource(name, interfaces[0], interfaces[1], parameters[0]));
        if (voltageSlider != null)
        {
            voltageSlider.value = voltage;
            voltageSlider.onValueChanged.AddListener(OnSliderValueChanged);
            voltageSlider.transform.position = transform.position + sliderOffset;
        }

        if (voltageValueText != null)
        {
            voltageValueText.text = voltage.ToString("F2") + " V";
            voltageValueText.transform.position = voltageSlider.transform.position + new Vector3(0, 0.5f, 0); // Adjust as needed
        }
    }

    private void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
        Circuit.componentValue = Parameters[0].ToString() + " V";
    }

    private void OnSliderValueChanged(float value)
    {
        base.Update();
        voltage = value;
        Parameters[0] = voltage;
        spiceEntitys[0] = new SpiceSharp.Components.VoltageSource(Name, Interfaces[0], Interfaces[1], voltage);
        if (voltageValueText != null)
        {
            voltageValueText.text = voltage.ToString("F2") + " V";
        }
        // Update Circuit display value
        Circuit.componentValue = voltage.ToString("F2") + " V";
        OnComponentChanged(this, new EventArgs());
    }
}
*/

/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpiceSharp.Components;

public class VariableVoltageSource : CircuitComponent
{
    private event EventHandler OnComponentChanged;
    public GameObject slider;
    public double Ratio = 0.5f;

    private SpiceSharp.Components.VoltageSource spiceVoltageSource;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceVoltageSource = new SpiceSharp.Components.VoltageSource(name, interfaces[0], interfaces[1], parameters[1] * Ratio);
        spiceEntitys.Add(spiceVoltageSource);
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

        if (slider == null)
        {
            Debug.LogError("Slider object is not assigned.");
            return;
        }

        var rheostatSlider = slider.GetComponent<RheostatSlider>();
        if (rheostatSlider == null)
        {
            Debug.LogError("RheostatSlider component is not found on the slider object.");
            return;
        }

        double sliderRatio = rheostatSlider.Ratio;
        if (Ratio != sliderRatio && spiceEntitys != null && spiceEntitys.Count > 0)
        {
            spiceVoltageSource.SetParameter("dc", Math.Max(sliderRatio * this.Parameters[1], this.Parameters[0]));

            OnComponentChanged?.Invoke(this, EventArgs.Empty);
        }
        Ratio = sliderRatio;
    }
}
*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpiceSharp.Components;

public class VariableVoltageSource : CircuitComponent
{
    private event EventHandler OnComponentChanged;
    public GameObject slider;
    public float Voltage = 0.0f;
    public const float MaxVoltage = 15.0f;
    public const float MinVoltage = 0.0f;

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();

        // Create a DC voltage source
        spiceEntitys.Add(new SpiceSharp.Components.VoltageSource(name, interfaces[0], interfaces[1], parameters[0]));
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

        float sliderVoltage = slider.GetComponent<VoltageSlider>().Voltage;
        if (Voltage != sliderVoltage && spiceEntitys != null)
        {
            // Update the voltage of the source
            spiceEntitys[0].SetParameter("dc", sliderVoltage);
            if (OnComponentChanged != null)
            {
                OnComponentChanged(this, new EventArgs());
            }
        }
        Voltage = sliderVoltage;
    }

    private void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
        Circuit.componentValue = string.Format("{0:0.##}", Voltage) + " V";
    }
}
