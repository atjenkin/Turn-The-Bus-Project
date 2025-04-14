using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderClamp : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private GameObject pencil;

    float imageFlip = 5f;

    float BlurValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener((v) =>
        {
            valueText.text = v.ToString("0.00");
            if (v > imageFlip) { 
                pencil.transform.rotation = Quaternion.EulerRotation(new Vector3(0,0,3.1f));
                pencil.transform.localPosition = new Vector3(0, 513, 0);
            }
            else { 
                pencil.transform.rotation = Quaternion.EulerRotation(new Vector3(0, 0, 0));
                pencil.transform.localPosition = new Vector3(0, -513, 0);
            }
            pencil.transform.localScale = new Vector2((1+v/5) + imageFlip / 5, (1 + v / 5) + imageFlip / 5);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void concave()
    {
        imageFlip = 5f;
        if (slider.value > imageFlip)
        {
            pencil.transform.rotation = Quaternion.EulerRotation(new Vector3(0, 0, 3.1f));
            pencil.transform.localPosition = new Vector3(0, 513, 0);
        }
        else
        {
            pencil.transform.rotation = Quaternion.EulerRotation(new Vector3(0, 0, 0));
            pencil.transform.localPosition = new Vector3(0, -513, 0);
        }
        pencil.transform.localScale = new Vector2((1 + slider.value / 5)+imageFlip/5, (1 + slider.value / 5) + imageFlip / 5);
    }
    public void water()
    {
        imageFlip = 7f;
        if (slider.value > imageFlip)
        {
            pencil.transform.rotation = Quaternion.EulerRotation(new Vector3(0, 0, 3.1f));
            pencil.transform.localPosition = new Vector3(0, 513, 0);
        }
        else
        {
            pencil.transform.rotation = Quaternion.EulerRotation(new Vector3(0, 0, 0));
            pencil.transform.localPosition = new Vector3(0, -513, 0);
        }
        pencil.transform.localScale = new Vector2((1 + slider.value / 5) + imageFlip / 5, (1 + slider.value / 5) + imageFlip / 5);
    }
    public void convex()
    {
        imageFlip = 10f;
        if (slider.value > imageFlip)
        {
            pencil.transform.rotation = Quaternion.EulerRotation(new Vector3(0, 0, 3.1f));
            pencil.transform.localPosition = new Vector3(0, 513, 0);
        }
        else
        {
            pencil.transform.rotation = Quaternion.EulerRotation(new Vector3(0, 0, 0));
            pencil.transform.localPosition = new Vector3(0, -513, 0);
        }
        pencil.transform.localScale = new Vector2((1 + slider.value / 5) + imageFlip / 5, (1 + slider.value / 5) + imageFlip / 5);
    }
}
