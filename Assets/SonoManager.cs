using UnityEngine;
using UnityEngine.UI;

public class SonoManager : MonoBehaviour
{
    public Transform pCube2;
    public Transform pCube6;
    public Slider mySlider2;
    public Slider mySlider6;

    private Vector3 startPos2;
    private Vector3 startPos6;
    private Vector3 midPoint;

    void Start()
    {
        // Save initial positions
        startPos2 = pCube2.position;
        startPos6 = pCube6.position;
        midPoint = (startPos2 + startPos6) / 2f;

        // Add listener for slider
        mySlider2.onValueChanged.AddListener(OnSliderChanged2);
        mySlider6.onValueChanged.AddListener(OnSliderChanged6);

    }

    void OnSliderChanged2(float value)
    {
        Debug.Log("moved 2 "+value);
        // value goes from 0 → 1
        // 0 = original positions, 1 = at midpoint
        pCube2.position = Vector3.Lerp(startPos2, midPoint, value);
    }

    void OnSliderChanged6(float value)
    {
        Debug.Log("moved 6 "+value);
        // value goes from 0 → 1
        // 0 = original positions, 1 = at midpoint
        pCube6.position = Vector3.Lerp(startPos6, midPoint, value);
    }
}
