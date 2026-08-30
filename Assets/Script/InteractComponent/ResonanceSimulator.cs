using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResonanceSimulator : MonoBehaviour
{
    [Header("Input Sliders")]
    public Slider pCube6Slider;   // 0–1, controls Bridge 2 position
    public Slider pCube2Slider;   // 0–1, controls Bridge 2 position

    public Slider weightSlider;   // 0–1, controls Mass

    [Header("Text Outputs")]
    public TextMeshProUGUI lengthText;
    public TextMeshProUGUI weightText;

    [Header("Output")]
    public Slider resonanceSlider;  // intensity meter (0–1)

    // Physical constants
    private const float mu = 0.0006f; // kg/m (wire mass per unit length)
    private const float g = 9.81f;    // gravity
    private const float f = 50f;      // Hz (frequency of source)
    private const float tolerance = 0.20f; // meters (20 cm, experimental margin)

    void Update()
    {
        // Fixed bridge
        float bridge1Pos = Mathf.Lerp(0.020f, 0.50f, pCube2Slider.value);

        // Movable bridge (30–100 cm)
        float bridge2Pos = Mathf.Lerp(0.020f, 0.50f, pCube6Slider.value);

        // Mass (50–250 g)
        float m = Mathf.Lerp(0.05f, 0.25f, weightSlider.value);

        // Effective length
        float L = Mathf.Abs(1f- bridge2Pos - bridge1Pos);

        // Tension and theoretical resonance length
        float T = m * g;
        float L_theoretical = (1f / (2f * f)) * Mathf.Sqrt(T / mu);

        // Closeness to resonance (0 → 1)
        float closeness = Mathf.Max(0f, 1f - Mathf.Abs(L - L_theoretical) / tolerance);

        // Show values in UI (cm and g)
        // bridge1Text.text = (bridge1Pos * 100f).ToString("F0") + " cm";
        // bridge2Text.text = (bridge2Pos * 100f).ToString("F0") + " cm";
        lengthText.text = ((1f- bridge2Pos - bridge1Pos) * 100f).ToString("F0") + " cm";
        weightText.text = (m * 1000f).ToString("F0") + " g";

        // Smooth update of resonance intensity slider
        resonanceSlider.value = Mathf.Lerp(resonanceSlider.value, closeness, Time.deltaTime * 5f);

        // Debug log (for checking in console)
        Debug.Log("Bridge1 = " + (bridge1Pos * 100f) + " cm | " +
                  "Bridge2 = " + (bridge2Pos * 100f) + " cm | " +
                  "Length L = " + (L * 100f).ToString("F1") + " cm | " +
                  "Mass = " + (m * 1000f).ToString("F0") + " g | " +
                  "Tension T = " + T.ToString("F2") + " N | " +
                  "L_theoretical = " + (L_theoretical * 100f).ToString("F1") + " cm | " +
                  "Resonance Intensity = " + closeness.ToString("F2"));
    }
}
