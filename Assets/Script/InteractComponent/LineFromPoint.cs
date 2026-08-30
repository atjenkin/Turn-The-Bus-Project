

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LineFromPoint : MonoBehaviour
{
    public GameObject startPoint;  // The point in 3D space (B - Batman)
    public GameObject topPoint; // Assign your top object here in the Inspector
    public GameObject bottomPoint; 
    public GameObject rayManager;   // The manager that controls ray direction and position
    public GameObject normalLineStart;
    public GameObject normalLineEnd;
    public GameObject triangleTopPoint;  // The top point of the triangle (P)
    public GameObject triangleBottomPoint;  // The bottom-right point of the triangle (Q)
    public GameObject lastNormalLineStart;
    public GameObject lastNormalLineEnd;
    public GameObject lastRayManager;
    public GameObject lastStartPoint;    
    public Slider angleSlider; // Slider to adjust the angle

    public TextMeshProUGUI incidentAngleText;
    public TextMeshProUGUI emergentAngleText;

    private float minSliderValue = 0.472f;
    private float maxSliderValue = 0.80f;
  
    void Start()
    {
        Vector3 lineStartPos = normalLineStart.transform.position;
        Vector3 lineEndPos = normalLineEnd.transform.position;        
        Vector3 lineDirection = lineEndPos - lineStartPos;

        Vector3 topPos = topPoint.transform.position;
        Vector3 bottomPos = bottomPoint.transform.position;
        Vector3 direction = bottomPos - topPos;

        Vector3 lastLineStartPos = lastNormalLineStart.transform.position;
        Vector3 lastLineEndPos = lastNormalLineEnd.transform.position;
        Vector3 lastLineDirection = lastLineEndPos - lastLineStartPos;

        float angle = Vector3.Angle(direction, lineDirection);
        angle = 180 - angle;

        float refracted = CalculateRefractionAngle(angle, 1.0f, 1.5f);
        float r2 = 60 - refracted;
        float emergent = CalculateRefractionAngle(r2, 1.5f, 1.0f);

        incidentAngleText.text = angle.ToString();
        emergentAngleText.text = emergent.ToString();

        Vector3 point = startPoint.transform.position; 
        Vector3 lastPoint = lastStartPoint.transform.position;

        CreateRayAtAngle(point, refracted, lineDirection);
        CreateRayAtAngleLast(lastPoint, r2, lastLineDirection);
    }

    void Update()
    {
        if (angleSlider.value < minSliderValue || angleSlider.value > maxSliderValue)
        {
            angleSlider.value = Mathf.Clamp(angleSlider.value, minSliderValue, maxSliderValue);
        }
        
        Vector3 lineStartPos = normalLineStart.transform.position;
        Vector3 lineEndPos = normalLineEnd.transform.position;        
        Vector3 lineDirection = lineEndPos - lineStartPos;
        
        Vector3 topPos = topPoint.transform.position;
        Vector3 bottomPos = bottomPoint.transform.position;
        Vector3 direction = bottomPos - topPos;

        Vector3 lastLineStartPos = lastNormalLineStart.transform.position;
        Vector3 lastLineEndPos = lastNormalLineEnd.transform.position;
        Vector3 lastLineDirection = lastLineEndPos - lastLineStartPos;

        float angle = Vector3.Angle(direction, lineDirection);
        angle = 180 - angle;

        float refracted = CalculateRefractionAngle(angle, 1.0f, 1.5f);
        float r2 = 60 - refracted;
        r2 = -r2;
        float emergent = CalculateRefractionAngle(r2, 1.5f, 1.0f);
        
        incidentAngleText.text = "Incident Angle: " + angle.ToString("F2") + "°";
        emergentAngleText.text = "Emergent Angle: " + (-emergent).ToString("F2") + "°";

        Vector3 point = startPoint.transform.position; 
        Vector3 lastPoint = lastStartPoint.transform.position;
        lastStartPoint.transform.position = rayManager.transform.position;

        CreateRayAtAngle(point, refracted, lineDirection);
        CreateRayAtAngleLast(lastPoint, emergent, lastLineDirection);
    }

    float CalculateRefractionAngle(float angle, float n1, float n2)
    {
        float angleInRadians = Mathf.Deg2Rad * angle;
        float sinTheta2 = (n1 / n2) * Mathf.Sin(angleInRadians);

        if (sinTheta2 > 1.0f || sinTheta2 < -1.0f)
        {
            Debug.LogWarning("Total Internal Reflection!");
            return 90f; // or handle reflection case
        }

        float angle2 = Mathf.Asin(sinTheta2);
        return Mathf.Rad2Deg * angle2; // Convert back to degrees
    }

    void CreateRayAtAngle(Vector3 startPoint, float angle, Vector3 referenceDirection)
    {
        float angleInRadians = angle * Mathf.Deg2Rad;
        referenceDirection.Normalize();

        Vector3 perpendicularDirection = new Vector3(-referenceDirection.y, referenceDirection.x, 0);
        Vector3 direction = referenceDirection * Mathf.Cos(angleInRadians) + perpendicularDirection * Mathf.Sin(angleInRadians);

        Vector3 intersectionPoint = FindIntersection(startPoint, direction, triangleTopPoint, triangleBottomPoint);

        if (intersectionPoint != Vector3.zero)
        {
            rayManager.transform.position = intersectionPoint;
            Debug.DrawLine(startPoint, intersectionPoint, Color.red, 10f); // Red line for 10 seconds
        }
    }

    void CreateRayAtAngleLast(Vector3 startPoint, float angle, Vector3 referenceDirection)
    {
        float angleInRadians = angle * Mathf.Deg2Rad;
        referenceDirection.Normalize();

        Vector3 perpendicularDirection = new Vector3(-referenceDirection.y, referenceDirection.x, 0);
        Vector3 direction = referenceDirection * Mathf.Cos(angleInRadians) + perpendicularDirection * Mathf.Sin(angleInRadians);
        if (direction.y > 0) direction.y = -direction.y;

        float distance = 100f;
        Vector3 newPoint = startPoint + direction * distance;
        lastRayManager.transform.position = newPoint;
    }

    Vector3 FindIntersection(Vector3 rayOrigin, Vector3 direction, GameObject pointP, GameObject pointQ)
    {
        Vector3 P = pointP.transform.position;
        Vector3 Q = pointQ.transform.position;
        Vector3 lineDirection = Q - P;

        float det = direction.x * lineDirection.y - direction.y * lineDirection.x;

        if (Mathf.Approximately(det, 0)) return Vector3.zero;

        float t = ((P.x - rayOrigin.x) * lineDirection.y - (P.y - rayOrigin.y) * lineDirection.x) / det;
        float s = ((P.x - rayOrigin.x) * direction.y - (P.y - rayOrigin.y) * direction.x) / det;

        if (s >= 0 && s <= 1) return rayOrigin + t * direction;

        return Vector3.zero;
    }
}
