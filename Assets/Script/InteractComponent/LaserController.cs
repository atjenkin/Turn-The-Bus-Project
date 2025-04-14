using UnityEngine;
using UnityEngine.UI;

public class LaserController : MonoBehaviour
{
    public Slider angleSlider; // Attach the slider here in Inspector
    public Transform pencil; // Attach the pencil (laser) object here

    private float minAngle = 30f;  // Minimum allowed angle
    private float maxAngle = 60f;  // Maximum allowed angle

    void Start()
    {
        Debug.Log("started"+angleSlider.minValue + minAngle);
        // Set the slider min and max values
        angleSlider.minValue = minAngle;
        angleSlider.maxValue = maxAngle;

        // Set initial value to minimum
        angleSlider.value = minAngle;
    }

    void Update()
    {
                Debug.Log("updated"+angleSlider.value + minAngle);

        // Apply the slider value to rotate the pencil
        float currentAngle = angleSlider.value;
        pencil.localRotation = Quaternion.Euler(0, 0, -currentAngle); // Rotate pencil on the z-axis
    }
}
