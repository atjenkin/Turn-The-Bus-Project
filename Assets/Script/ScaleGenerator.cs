using UnityEngine;
using TMPro;

public class ScaleMarkerGenerator : MonoBehaviour
{
    [SerializeField] private GameObject markerPrefab; // e.g., a vertical line or tick
    [SerializeField] private TextMeshProUGUI labelPrefab;
    [SerializeField] private RectTransform parent;    // Parent under Canvas

    [SerializeField] private int maxCM = 60;
    [SerializeField] private int minCM = -60;
    [SerializeField] private int stepCM = 10;

    private const float cmToUnityX = 12.5f; // 1 cm = -12.5 units in Unity

    void Start()
    {
        GenerateScale();
    }

    private void GenerateScale()
    {
        for (int cm = minCM; cm <= maxCM; cm += stepCM)
        {
            float x = CmToX(cm);

            // Instantiate tick mark
            if (markerPrefab != null)
            {
                GameObject marker = Instantiate(markerPrefab, parent);
                marker.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
                marker.name = $"Marker_{cm}cm";
            }

            // Instantiate label
            if (labelPrefab != null)
            {
                TextMeshProUGUI label = Instantiate(labelPrefab, parent);
                label.text = cm.ToString();
                RectTransform rect = label.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(x, -20f);  // Adjust Y offset to appear below tick
                label.name = $"Label_{cm}cm";
            }
        }
    }

    private float CmToX(float cm)
    {
        return cm * cmToUnityX;
    }
}
