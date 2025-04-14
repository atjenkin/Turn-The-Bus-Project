using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pencil_Slider : MonoBehaviour
{
    [SerializeField] RawImage pencil_Image;
    [SerializeField] Slider pencil;
    [SerializeField] TextMeshProUGUI u;
    [SerializeField] TextMeshProUGUI v;
    // Start is called before the first frame update
    void Start()
    {
        pencil_Image.rectTransform.anchoredPosition = new Vector2(448f-20*28.9f,71.1f);
        pencil_Image.rectTransform.sizeDelta = new Vector2(360.7f, 160f);
        u.text = "U : 20cm";
        v.text = "V : 20cm";
    }

    // Update is called once per frame
    void Update()
    {
        float U = 47.88f - pencil.value * 47.88f;
        float V = 10 * U / (U - 10);
        u.text = "U : " + string.Format("{0:0.##}", U) + "cm";
        v.text = "V : " + string.Format("{0:0.##}",V) + "cm";

        pencil_Image.rectTransform.pivot = new Vector2(0, 0);
        pencil_Image.rectTransform.sizeDelta = new Vector2(360.7f, Math.Abs(160f*V/U));
        if (V > 0)
        {
            pencil_Image.rectTransform.rotation = Quaternion.Euler(0, 0, 178);
            pencil_Image.rectTransform.anchoredPosition = new Vector2(448f-V*28.9f+175,0);
        }
        else
        {
            pencil_Image.rectTransform.rotation = Quaternion.Euler(0, 0, -2.18f);
            pencil_Image.rectTransform.anchoredPosition = new Vector2(448f - V * 28.9f-175,0);
        }

        if (U < 10) pencil_Image.GetComponent<RawImage>().color = new Color32(45, 100, 117,200);
        else pencil_Image.GetComponent<RawImage>().color = new Color32(211, 140, 140,200);

        if(U<11.5 && U>8.5) v.text = "V : \u221e cm";
    }
}
