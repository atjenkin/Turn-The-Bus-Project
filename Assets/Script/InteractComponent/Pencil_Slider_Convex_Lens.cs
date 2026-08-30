 using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pencil_Slider_Convex_Lens : MonoBehaviour
{
    [SerializeField] private RawImage pencil_Image;
    [SerializeField] private Slider pencil;
    [SerializeField] private TextMeshProUGUI u;
    [SerializeField] private TextMeshProUGUI v;

    private const float focalLength = 10f; // cm

    void Start()
    {
        if (pencil_Image != null)
        {
            float initialU = 60f;
            pencil.value = 0f; // slider leftmost
            pencil_Image.rectTransform.anchoredPosition = new Vector2(CmToXObject(-initialU), 0);
            pencil_Image.rectTransform.sizeDelta = new Vector2(360.7f, 160f);
        }

        u.text = "U : 60cm";
        v.text = "V : ?";
    }

    void Update()
    {
        if (pencil == null || pencil_Image == null || u == null || v == null)
        {
            Debug.LogError("Missing UI references.");
            return;
        }

        // Slider maps: 0 → 60cm, 1 → 0cm
        float U = (float)Math.Round(60f * (1f - pencil.value), 1);
        float u_real = -U;  // Object on left → negative
        u.text = $"U : {-1 *U:0.0}cm";

        float f = focalLength;
        float v_real;
        bool isInfinity = false;

        if (Math.Abs(U - f) < 0.01f)
        {
            v_real = float.PositiveInfinity;
            v.text = "V : ∞ cm";
            isInfinity = true;
        }
        else
        {
            v_real = 1f / ((1f / f) + (1f / u_real)); // signed image distance
            v_real = (float)Math.Round(v_real, 1);  // Round to 1 decimal
            v.text = $"V : {Math.Abs(v_real):0.0}cm";
        }

        float magnification = isInfinity ? 1f : v_real / u_real;
        float imageHeight = Math.Abs(160f * magnification);
        pencil_Image.rectTransform.sizeDelta = new Vector2(360.7f, imageHeight);
        pencil_Image.rectTransform.pivot = new Vector2(0, 0);

        float imageX;
        if (isInfinity)
        {
            imageX = CmToXImage(60f); // simulate far right
            pencil_Image.rectTransform.rotation = Quaternion.identity;
        }
        else if (v_real > 0)
        {
            // Real image → right side → +X → inverted
            imageX = CmToXImage(v_real);
            pencil_Image.rectTransform.rotation = Quaternion.Euler(0, 0, 178);
        }
        else
        {
            // Virtual image → same side (left side) → -X → upright
            imageX = CmToXImage(v_real);
            pencil_Image.rectTransform.rotation = Quaternion.identity;
            v.text = $"V : {-1 * Math.Abs(v_real):0.0}cm";
        }

        pencil_Image.rectTransform.anchoredPosition = new Vector2(imageX, 0);

        pencil_Image.color = U < f
            ? new Color32(45, 100, 117, 200)   // virtual
            : new Color32(211, 140, 140, 200); // real

        float objectX = CmToXObject(u_real);
        //Debug.Log($"[Optics Debug] U = {U:0.##} cm → Unity X = {objectX:0.##} | V = {(isInfinity ? "∞" : v_real.ToString("0.##"))} cm → Unity X = {imageX:0.##}");
    }

    // Object on left (U) → Unity -X
    private float CmToXObject(float cm)
    {
        return -12.5f * cm;
    }

    // Image:
    // - real image (v > 0) → Unity +X
    // - virtual image (v < 0) → Unity -X
    private float CmToXImage(float v_cm)
    {
         if (v_cm > 0)
            return (12.5f * v_cm) + 150f;   // real → right side of lens
        else
            return (12.5f * v_cm) - 150f;   // virtual → left side of lens
    }
}
