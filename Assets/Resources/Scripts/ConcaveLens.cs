using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ConcaveLens : MonoBehaviour
{
    public GameObject pencil; // The object needle
    public GameObject convexLens; // The convex lens
    public GameObject concaveLens; // The concave lens
    public GameObject opticalCenter; // The optical center of the lens
    public GameObject principalAxis; // LineRenderer for principal axis
    public Slider lensPositionSlider; // Slider to adjust lens position
    public TMP_Text finalImageText; // Text to display the final image information

    private GameObject imageNeedle; // The image needle
    private Vector3 initialLensPosition;
    private Vector3 initialConcavePosition;

    private float objectDistanceFromLens = 3.0f; // Fixed object distance from the lens
    private float focalLengthLens = 2.0f; // Focal length of the lens
    private float focalLengthConcave = -2.0f; // Focal length of the concave lens

    void Start()
    {
        // Verify if all required GameObjects are assigned
        if (pencil == null || convexLens == null || concaveLens == null || opticalCenter == null || principalAxis == null || lensPositionSlider == null || finalImageText == null)
        {
            Debug.LogError("One or more required GameObjects are not assigned in the Inspector.");
            return;
        }

        initialLensPosition = convexLens.transform.position;
        initialConcavePosition = concaveLens.transform.position;

        // Initialize slider
        if (lensPositionSlider != null)
        {
            lensPositionSlider.onValueChanged.AddListener(OnLensPositionChanged);
            lensPositionSlider.minValue = 1;
            lensPositionSlider.maxValue = 5; // Adjust based on your scene setup
            lensPositionSlider.value = 2; // Set a default value
            Debug.Log("Lens position from lens:" + lensPositionSlider);

            // Add event triggers for OnPointerDown and OnDrag
            AddEventTrigger(lensPositionSlider.gameObject, EventTriggerType.PointerDown, (eventData) => { Debug.Log("Lens Position Slider Pointer Down"); });
            AddEventTrigger(lensPositionSlider.gameObject, EventTriggerType.Drag, (eventData) => { Debug.Log("Lens Position Slider Dragging"); });
        }

        // Instantiate the image needle as a clone of the pencil
        imageNeedle = Instantiate(pencil);
        imageNeedle.name = "ImageNeedle";
        imageNeedle.GetComponent<SpriteRenderer>().color = pencil.GetComponent<SpriteRenderer>().color;

        // Position the optical center at the lens position
        if (opticalCenter != null)
        {
            opticalCenter.transform.position = initialLensPosition;
            //AddLabelToPoint(opticalCenter, "LabelC", "C");
        }

        // Draw the principal axis
        DrawPrincipalAxis();

        // Mark basic points on the principal axis
        MarkPrincipalAxisPoints();

        // Set initial positions
        OnLensPositionChanged(lensPositionSlider.value);
    }

    void OnLensPositionChanged(float value)
    {
        Debug.Log("Slider Value Changed: " + value);
        UpdateLensPosition(value);
        CalculateFinalImage();
    }

    void UpdateLensPosition(float value)
    {
        if (concaveLens == null) return;

        // Move the lens along the principal axis (x-axis)
        concaveLens.transform.position = initialLensPosition + new Vector3(value, 0, 0);
        Debug.Log("Lens Position Updated: " + concaveLens.transform.position);
    }

    void CalculateFinalImage()
    {
        if (pencil == null || convexLens == null || concaveLens == null || imageNeedle == null) return;

        // Position the pencil (object) at a fixed distance from the lens, just below the principal axis
        pencil.transform.position = initialLensPosition - new Vector3(objectDistanceFromLens, 0.5f, 0);
        // Calculate the intermediate image distance using the lens formula: 1/f = 1/do + 1/di
        float intermediateImageDistance = 1 / (1 / focalLengthLens + 1 / objectDistanceFromLens);

        // Calculate the position of the intermediate image
        Vector3 intermediateImagePosition = new Vector3(intermediateImageDistance, 0, 0);
        // Calculate the virtual object distance for the concave lens (distance from the lens to the intermediate image)
        float virtualObjectDistance = concaveLens.transform.position.x -  intermediateImagePosition.x ;
        // Debug.Log("finalImageDistancem "+focalLengthConcave+" "+virtualObjectDistance);

        // Calculate the refracted image distance using the lens formula: 1/f = 1/do + 1/di
        float finalImageDistance = 1 / ((1 / focalLengthConcave) + (1 / virtualObjectDistance));
        // Debug.Log("finalImageDistancem2 "+finalImageDistance);

        Vector3 finalImagePosition = concaveLens.transform.position + new Vector3(finalImageDistance, 0, 0);
        float finalImageDistanceFromLens = Vector3.Distance(initialLensPosition, concaveLens.transform.position) + finalImageDistance;
        //float focalLengthConcave = 1 / (1 / (6.0 - concaveLens.transform.position) + 1 / finalImageDistance);

        // Move the image needle to the final image position
        imageNeedle.transform.position = new Vector3(finalImagePosition.x, -pencil.transform.position.y - 0.2f, pencil.transform.position.z);

        // Display the final image information or indicate coincidence
        if (finalImageText != null)
        {
            if (Mathf.Abs(finalImageDistance - objectDistanceFromLens) < -0.1f) // Allowing a small margin for floating point precision
            {
                //finalImageText.text = "The tips of Image and Object needle coincides\nFocal length of concave lens is 1";
            }
            else
            {
                finalImageText.text = 
                    "Final Image Distance (v) from Concave Lens: " + (Mathf.Ceil((finalImageDistance*5) * 10f) / 10f).ToString("F1") + "\n" +
                    "Virtual Object Distance (u) from Concave Lens: " + (Mathf.Ceil((virtualObjectDistance*5) * 10f) / 10f).ToString("F1") + "\n" +
                    "Focal length (1/v-1/u): " + (Mathf.Ceil((focalLengthConcave*5) * 10f) / 10f).ToString("F1");

            }
        }

        Debug.Log("Intermediate Image Position: " + intermediateImagePosition);
        Debug.Log("Final Image Distance: " + finalImageDistance);
        Debug.Log("virtualObjectDistance " + virtualObjectDistance);
        //Debug.Log("Final Image Position: " + finalImagePosition);
    }

    void DrawPrincipalAxis()
    {
        if (principalAxis == null) return;

        LineRenderer lr = principalAxis.GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(-20, 0, 0)); // Start point of the line
            lr.SetPosition(1, new Vector3(20, 0, 0)); // End point of the line
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
        }
    }

    void MarkPrincipalAxisPoints()
    {
        AddLabelToPoint(new GameObject("F1"), "LabelF1", "F1", initialLensPosition + new Vector3(focalLengthLens, 0, 0));
        AddLabelToPoint(new GameObject("2F1"), "Label2F1", "2F1", initialLensPosition + new Vector3(2 * focalLengthLens, 0, 0));
        AddLabelToPoint(new GameObject("F2"), "LabelF2", "F2", initialLensPosition - new Vector3(focalLengthLens, 0, 0));
        AddLabelToPoint(new GameObject("2F2"), "Label2F2", "2F2", initialLensPosition - new Vector3(2 * focalLengthLens, 0, 0));
        AddLabelToPoint(new GameObject("O"), "LabelO", "O", initialLensPosition);
        AddLabelToPoint(pencil, "LabelAB", "AB", pencil.transform.position);
        AddLabelToPoint(imageNeedle, "LabelA'B'", "A'B'", imageNeedle.transform.position);
    }

    void AddEventTrigger(GameObject obj, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null) trigger = obj.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    void AddLabelToPoint(GameObject point, string labelName, string labelText, Vector3 position)
    {
        if (point == null) return;

        point.transform.position = position;

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

