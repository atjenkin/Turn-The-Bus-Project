using UnityEngine;
using UnityEngine.UI;

public class SliderLinker : MonoBehaviour
{
    public Slider parentSlider; // Assign the parent slider in the Inspector
    public Slider childSlider;  // Assign the child slider in the Inspector

    void Start()
    {
        // Optionally ensure the child slider matches the parent on start
        childSlider.value = parentSlider.value;
    }

    void Update()
    {
        // Update the child slider value to match the parent slider each frame
        if (parentSlider != null && childSlider != null)
        {
            childSlider.value = parentSlider.value;
        }
    }
}
