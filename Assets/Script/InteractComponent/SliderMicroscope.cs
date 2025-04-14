using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SliderMicroscope : MonoBehaviour
{
    [SerializeField] private Slider MValue;
    [SerializeField] private Slider NValue;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private GameObject blur;

    [SerializeField] private GameObject cross;
    [SerializeField] private GameObject slab;
    [SerializeField] private GameObject powder;

    [SerializeField] private Slider Thickness;
    [SerializeField] private TextMeshProUGUI ThicknessValue;

    float thickness = 1;
    float m = 0;
    float n = 0;
    float BlurValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        blur.GetComponent<Image>().material.SetFloat("_Size", 8);

        MValue.onValueChanged.AddListener((v) =>
        {
            m = v * 1/20f;
            valueText.text = (m+n).ToString("0.00")+" mm";
            blur.GetComponent<Image>().material.SetFloat("_Size", ((m+n)-(2+thickness+BlurValue))*3);
        });

        NValue.onValueChanged.AddListener((v) =>
        {
            n = v * 0.001f;
            valueText.text = (m+n).ToString("0.000")+" mm";
            blur.GetComponent<Image>().material.SetFloat("_Size", ((m + n) - (2 + thickness + BlurValue)) * 3);
        });

        Thickness.onValueChanged.AddListener((v) =>
        {
            thickness = v;
            ThicknessValue.text = v.ToString("0")+" mm";
            blur.GetComponent<Image>().material.SetFloat("_Size", ((m + n) - (2 + thickness + BlurValue)) * 3);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void crossActivated()
    {
        BlurValue = -1f;
        blur.GetComponent<Image>().material.SetFloat("_Size", ((m + n) - (2 + thickness + BlurValue)) * 3);
    }
    public void slabActivated()
    {
        BlurValue = -thickness+thickness/1.5f;
        blur.GetComponent<Image>().material.SetFloat("_Size", ((m + n) - (2 + thickness + BlurValue)) * 3);
    }
    public void powderActivated()
    {
        BlurValue = 0f;
        blur.GetComponent<Image>().material.SetFloat("_Size", ((m + n) - (2 + thickness + BlurValue)) * 3);
    }
}
