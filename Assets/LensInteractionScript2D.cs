using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LensInteractionScript2D : MonoBehaviour
{
    public GameObject pencil;
    public GameObject pointF1;
    public GameObject point2F1;
    public GameObject pointF2;
    public GameObject point2F2;
    public GameObject opticalCenter;
    public GameObject principalAxis; // LineRenderer for principal axis
    public Slider objectDistanceSlider;
    public Slider focalLengthSlider;
    public TMP_Text objectDistanceText;
    public TMP_Text focalLengthText;
    public TMP_Text imageDistanceText;
    public Graphic graph;

    private GameObject image;

    private Vector3 lensPosition = new Vector3(0, 0, 0);

    void Start()
    {
        // Initialize sliders
        if (objectDistanceSlider != null)
        {
            objectDistanceSlider.onValueChanged.AddListener(OnObjectDistanceChanged);
        }

        if (focalLengthSlider != null)
        {
            focalLengthSlider.onValueChanged.AddListener(OnFocalLengthChanged);
            if (focalLengthSlider.value == 0)
            {
                focalLengthSlider.value = 0.5f; // Set default value
            }
        }

        // Add event triggers for OnPointerDown and OnDrag
        AddEventTrigger(focalLengthSlider.gameObject, EventTriggerType.PointerDown, (eventData) => { Debug.Log("Focal Length Slider Pointer Down"); });
        AddEventTrigger(focalLengthSlider.gameObject, EventTriggerType.Drag, (eventData) => { Debug.Log("Focal Length Slider Dragging"); });

        // Create an image of the pencil
        image = Instantiate(pencil);
        //image.GetComponent<SpriteRenderer>().color = Color.blue; // Different color to distinguish
        // Set the image color to be the same as the pencil
        image.GetComponent<SpriteRenderer>().color = pencil.GetComponent<SpriteRenderer>().color;

        // Position the optical center at the lens position
        if (opticalCenter != null)
        {
            opticalCenter.transform.position = lensPosition;
            AddLabelToPoint(opticalCenter, "LabelC", "C");
        }

        // Draw the principal axis
        DrawPrincipalAxis();

        // Set initial positions
        UpdateLabelPositions();
        OnObjectDistanceChanged(objectDistanceSlider.value);
        OnFocalLengthChanged(focalLengthSlider.value);
    }

    void OnObjectDistanceChanged(float value)
    {
        UpdatePositions();
        UpdateTexts();
    }

    void OnFocalLengthChanged(float value)
    {
        UpdatePositions();
        UpdateTexts();
        UpdateLabelPositions();
    }

    /*
        void UpdatePositions()
        {
            if (pencil == null || image == null) return;

            // Get slider values
            float objectDistance = objectDistanceSlider != null ? objectDistanceSlider.value : 0;
            float focalLength = focalLengthSlider != null ? focalLengthSlider.value : 0;

            if (objectDistance == 0 || objectDistance == focalLength)
            {
                image.transform.position = new Vector3(lensPosition.x, 0.5f, lensPosition.z);
                if (imageDistanceText != null)
                    imageDistanceText.text = "Image Distance (v): Undefined";
                return;
            }

            // Position the pencil based on object distance
            pencil.transform.position = new Vector3(lensPosition.x - objectDistance, 0.5f, lensPosition.z);

            // Image distance using lens formula (1/f = 1/do + 1/di)
            float imageDistance = 1 / (1 / focalLength - 1 / objectDistance);

            // Position the image on the opposite side of the lens
            image.transform.position = new Vector3(lensPosition.x + imageDistance, 0.5f, pencil.transform.position.z);

            // Scale the image to reflect inversion (if needed)
            float imageScaleY = objectDistance < focalLength ? -1 : 1;
            image.transform.localScale = new Vector3(pencil.transform.localScale.x, imageScaleY * pencil.transform.localScale.y, pencil.transform.localScale.z);
        }
        */

    void UpdatePositions()
{
    if (pencil == null || image == null) return;

    // Get slider values
    float objectDistance = objectDistanceSlider != null ? objectDistanceSlider.value : 0;
    float focalLength = focalLengthSlider != null ? focalLengthSlider.value : 0;

    // Update object distance text
    if (objectDistanceText != null)
    {
        objectDistanceText.text = "Object Distance (u): " + objectDistance.ToString("F2");
    }

    // Handle edge cases
    if (objectDistance == 0)
    {
        image.transform.position = new Vector3(lensPosition.x, 0.5f, lensPosition.z);
        if (imageDistanceText != null)
            imageDistanceText.text = "Image Distance (v): Undefined";
        return;
    }

    // Position the pencil based on object distance
    pencil.transform.position = new Vector3(lensPosition.x - objectDistance, 0.5f, lensPosition.z);

    // Image distance using lens formula (1/f = 1/do + 1/di)
    float imageDistance = 1 / (1 / focalLength - 1 / objectDistance);

    graph.AddPointXY (objectDistance,imageDistance);

    // Adjust for each case
    if (objectDistance > 2 * focalLength)
    {
        // Object beyond 2F: image is inverted, smaller, real
        image.transform.position = new Vector3(lensPosition.x + imageDistance, -0.5f, pencil.transform.position.z);
        image.transform.localScale = new Vector3(pencil.transform.localScale.x, -pencil.transform.localScale.y, pencil.transform.localScale.z);
    }
    else if (objectDistance == 2 * focalLength)
    {
        // Object at 2F: image is inverted, same sized, real
        image.transform.position = new Vector3(lensPosition.x + imageDistance, -0.5f, pencil.transform.position.z);
        image.transform.localScale = pencil.transform.localScale;
        image.transform.localScale = new Vector3(pencil.transform.localScale.x, -pencil.transform.localScale.y, pencil.transform.localScale.z);
    }
    else if (objectDistance > focalLength && objectDistance < 2 * focalLength)
    {
        // Object between F and 2F: image is enlarged, inverted, real
        image.transform.position = new Vector3(lensPosition.x + imageDistance, -0.5f, pencil.transform.position.z);
        image.transform.localScale = new Vector3(pencil.transform.localScale.x * (Mathf.Abs(imageDistance) / objectDistance), -pencil.transform.localScale.y * (Mathf.Abs(imageDistance) / objectDistance), pencil.transform.localScale.z);
    }
    else if (objectDistance == focalLength)
    {
        // Object at F: image is infinitely large, inverted, real
        image.transform.position = new Vector3(lensPosition.x + imageDistance, -0.5f, pencil.transform.position.z);
        // Here we simulate an infinitely large image
        image.transform.localScale = new Vector3(pencil.transform.localScale.x * 10, -pencil.transform.localScale.y * 10, pencil.transform.localScale.z); // Arbitrarily large size for visualization
    }
    else if (objectDistance < focalLength)
    {
        // Object distance < f: image is larger, erect, virtual
        float virtualImageDistance;
        if (Mathf.Abs(focalLength - objectDistance) > float.Epsilon)
        {
            virtualImageDistance = focalLength * (1 + (focalLength / (focalLength - objectDistance)));
        }
        else
        {
            virtualImageDistance = float.MaxValue; // or some large value to position it far away
        }

        if (objectDistance < 0)
        {
            image.transform.position = new Vector3(lensPosition.x - objectDistance, 0.5f, lensPosition.z + virtualImageDistance);
        }
        else
        {
            image.transform.position = new Vector3(lensPosition.x - objectDistance, 0.5f, lensPosition.z - virtualImageDistance);
        }
        image.transform.localScale = new Vector3(pencil.transform.localScale.x * (Mathf.Abs(virtualImageDistance) / Mathf.Abs(objectDistance)), pencil.transform.localScale.y * (Mathf.Abs(virtualImageDistance) / Mathf.Abs(objectDistance)), pencil.transform.localScale.z);
    }

    // Update image distance text
    if (imageDistanceText != null)
    {
        imageDistanceText.text = "Image Distance (v): " + imageDistance.ToString("F2");
    }
}



    void UpdateTexts()
    {
        float objectDistance = objectDistanceSlider != null ? objectDistanceSlider.value : 0;
        float focalLength = focalLengthSlider != null ? focalLengthSlider.value : 0;

        if (objectDistanceText != null)
            objectDistanceText.text = "Object Distance (u): " + objectDistance.ToString("F2");
        if (focalLengthText != null)
            focalLengthText.text = "Focal Length (f): " + focalLength.ToString("F2");

        if (imageDistanceText != null)
        {
            if (objectDistance == 0 || objectDistance == focalLength)
            {
                imageDistanceText.text = "Image Distance (v): Undefined";
            }
            else
            {
                float imageDistance = 1 / (1 / focalLength - 1 / objectDistance);
                imageDistanceText.text = "Image Distance (v): " + imageDistance.ToString("F2");
            }
        }
    }

    void UpdateLabelPositions()
    {
        float focalLength = focalLengthSlider != null ? focalLengthSlider.value : 0.5f;

        // Update F and 2F positions on both sides of the lens
        if (pointF1 != null) pointF1.transform.position = lensPosition - new Vector3(focalLength, 0, 0);  // F1 is at -focalLength
        if (point2F1 != null) point2F1.transform.position = lensPosition - new Vector3(2 * focalLength, 0, 0);  // 2F1 is at -2 * focalLength
        if (pointF2 != null) pointF2.transform.position = lensPosition + new Vector3(focalLength, 0, 0);  // F2 is at focalLength
        if (point2F2 != null) point2F2.transform.position = lensPosition + new Vector3(2 * focalLength, 0, 0);  // 2F2 is at 2 * focalLength

        // Update label positions relative to their points
        UpdateLabelPosition(pointF1, "LabelF1");
        UpdateLabelPosition(point2F1, "Label2F1");
        UpdateLabelPosition(pointF2, "LabelF2");
        UpdateLabelPosition(point2F2, "Label2F2");
        UpdateLabelPosition(opticalCenter, "LabelC");
    }

    void UpdateLabelPosition(GameObject point, string labelName)
    {
        if (point == null) return;

        Transform labelTransform = point.transform.Find(labelName);
        if (labelTransform != null)
        {
            labelTransform.position = point.transform.position + new Vector3(0, -0.1f, 0);
        }
    }

    void DrawPrincipalAxis()
    {
        if (principalAxis != null)
        {
            LineRenderer lr = principalAxis.GetComponent<LineRenderer>();
            if (lr != null)
            {
                lr.positionCount = 2;
                lr.SetPosition(0, new Vector3(-10, 0, 0)); // Start point of the line
                lr.SetPosition(1, new Vector3(10, 0, 0)); // End point of the line
            }
        }
    }

    void AddEventTrigger(GameObject obj, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null) trigger = obj.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    void AddLabelToPoint(GameObject point, string labelName, string labelText)
    {
        if (point == null) return;

        GameObject labelObject = new GameObject(labelName);
        labelObject.transform.SetParent(point.transform);
        labelObject.transform.localPosition = new Vector3(0, -0.1f, 0);

        TextMeshPro label = labelObject.AddComponent<TextMeshPro>();
        label.text = labelText;
        label.fontSize = 3;
        label.color = Color.black;
        label.alignment = TextAlignmentOptions.Center;
    }
}
