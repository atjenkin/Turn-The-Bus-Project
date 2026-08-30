using UnityEngine;
using UnityEngine.UI;
using TMPro;   // needed for TextMeshPro

public class SliderValueDisplay : MonoBehaviour
{
    public Slider mySlider;         // reference to the slider
    public TextMeshProUGUI valueText; // reference to the UI text

    void Start()
    {
        // Update text initially
        UpdateValue(mySlider.value);

        // Add listener for slider changes
        mySlider.onValueChanged.AddListener(UpdateValue);
    }

    void UpdateValue(float value)
    {
        valueText.text = value.ToString("F2");
    }
}
