
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecondAngleController : MonoBehaviour
{
    // Reference to the top and bottom objects
    public GameObject bottomObject; //The third object whose position depends on the tripled angle
    public GameObject differentsliderObject; // The third object whose position depends on the tripled angle
    public GameObject copysliderObject1; // The third object whose position depends on the tripled angle
    public GameObject copysliderObject2; // The third object whose position depends on the tripled angle
    public Slider outputslider;
    public Slider angleSlider; // Slider to reflect the tripled angle
    public GameObject outputhandlesliderObject;
    public TextMeshProUGUI angleText;
    public TextMeshProUGUI U;
    public TextMeshProUGUI V;

    void Update()
    {

        // Get positions of both objects
        Vector3 bottomPosition = bottomObject.transform.position; // Added to obtain the third object's position
        Vector3 differentsliderPosition = differentsliderObject.transform.position;
        Vector3 normallinePosition1 = copysliderObject1.transform.position;
        Vector3 normallinePosition2 = copysliderObject2.transform.position;
        Vector3 outputhandlesliderPosition = outputhandlesliderObject.transform.position;
        // Vector3 copysliderPosition = copysliderObject.transform.position;
        Debug.Log("i am fooooooooooool"+outputhandlesliderPosition);
        // Log the names and positions of bottom and third objects
        // Debug.Log("Bottom Object: " + bottomObject.name + ", Position: " + bottomPosition);
        // Debug.Log("Third Object: " + thirdObject.name + ", Position: " + thirdPosition);

        // Calculate the direction from topObject to bottomObject
        // Vector3 direction = bottomPosition - topPosition;
        Vector3 verticalAxis = Vector3.up;
        Vector3 newdirection = differentsliderPosition - bottomPosition;
        Vector3 outputDirection = outputhandlesliderPosition - differentsliderPosition;
        Vector3 normallinePosition = differentsliderPosition - normallinePosition1;
        float angle = 180 - Vector3.Angle(newdirection, normallinePosition) ;
        float outputAngle = 180  - Vector3.Angle(outputDirection,normallinePosition);
        Debug.Log("-----------------------");
        Debug.Log(outputDirection + "...." + outputslider.value + "......"+outputAngle);
        // Debug.Log(differentsliderPosition+"..............."+normallinePosition1);

        // Debug.Log("bottom position"+ bottomPosition + "slider position" + differentsliderPosition  +)
        float n1 = 1.5f; // Refractive index of air
        float n2 = 1.0f; // Refractive index of the prism material
        // Call the function to get the tripled angle
        float tripledAngle = CalculateRefractionAngle(angle, n1, n2);
// here want to have angle of output, made with two line, first is normal plane, that we have, and second is output line with slider
// first we need to print the angle that needs to be there
        angleSlider.value = tripledAngle;
        
        float maxAngle = 72.58f; // Maximum value of the angle
         float maxSliderValue = 0.66f; // Maximum value of the slider
        //  outputslider.minValue = 0.49f;
        //  outputslider.maxValue = 0.62f;
         // at this point set the value of outputslider.value
        // outputslider.value =(tripledAngle / maxAngle) * maxSliderValue;
        float normalizedAngle = tripledAngle / maxAngle; // Normalize to range [0, 1]
        outputslider.value = Mathf.Lerp( 0.49f, 0.62f , normalizedAngle);
        Debug.Log("sssssssssssssssssssss"+angle);
        Debug.Log("eeeeeeeeeeeeeeeeeeee (theory)"+tripledAngle);
        Debug.Log("eeeeeeeeeeeeeeeeeeee (real)"+outputAngle);

    //     // Vector3 refrectiveAngle = 

    //     // Define the vertical axis
    //     Vector3 verticalAxis = Vector3.up;
    //     // Calculate the angle between the direction and the vertical axis
    //     float angle = Vector3.Angle(direction, normallinePosition) ;

    //     // angle = -angle;
    //     angle = angle -90;

    //     float refrectiveAngle = Vector3.Angle(newdirection, normallinePosition) ;
    //     float adjustedAngle = refrectiveAngle - 175;
       
    //     Debug.Log(refrectiveAngle + "....="+adjustedAngle+".....................................");
    //     // float newangle = Vector3.Angle(direction, verticalAxis) - 90;
    //     float newangle = Vector3.Angle(newdirection, verticalAxis) - 90;

    //     Debug.Log(newangle +" ======"+ newdirection+" ----------------------------");
    //     // Debug.Log("bottom position"+ bottomPosition + "slider position" + differentsliderPosition  +)
    //     float n1 = 1.0f; // Refractive index of air
    //     float n2 = 1.5f; // Refractive index of the prism material
    //     // Call the function to get the tripled angle
    //     float tripledAngle = CalculateRefractionAngle(angle, n1, n2);

    //     // Update the angle text
    //     angleText.text = "Angle of Inc: " + angle.ToString("F2") + "°";
    //  float maxAngle = 41.81f; // Maximum value of the angle
    //      float maxSliderValue = 0.66f; // Maximum value of the slider

    //     // Set the slider's value to the tripled angle
    //     angleSlider.value =(tripledAngle / maxAngle) * maxSliderValue;

    //     // Position the third object based on the tripled angle (example: moving along the y-axis)
    //     // thirdObject.transform.position = bottomPosition + new Vector3(0, Mathf.Sin(tripledAngle * Mathf.Deg2Rad), Mathf.Cos(tripledAngle * Mathf.Deg2Rad));

    //     // Calculate and log the angle between the bottom and the third object
    //     // Vector3 directionToThird = thirdPosition - bottomPosition;
    //     float distance = CalculateDistance(bottomObject.transform.position, differentsliderObject.transform.position);
    //     Debug.Log("cccccccccangle slider value cccccccccccccccc   " + angleSlider.value);
    //     // float angleToThird = Vector3.Angle(directionToThird, direction); // Calculate the angle relative to the initial direction
    //     // Debug.Log("Angle between bottom and third objects: " + angleToThird + "°");
    //     Vector3 newPosition = CalculatePositionFromAngle(bottomPosition, tripledAngle, distance);
    //     // differentsliderObject.transform.position = newPosition;
    //     // Log the tripled angle for verification
    //     Debug.Log("Tripled angle: " + bottomPosition + tripledAngle);
    }


      Vector3 CalculatePositionFromAngle(Vector3 startPoint, float angle, float distance)
    {
        // Convert angle to radians
        float angleRad = angle * Mathf.Deg2Rad;

        // Calculate new position
        float x = Mathf.Cos(angleRad) * distance;
        float y = Mathf.Sin(angleRad) * distance;

        // Assuming angle is with respect to the horizontal axis
        return new Vector3(startPoint.x + x, startPoint.y + y, startPoint.z);
    }

    float CalculateRefractionAngle(float angleOfIncidence, float n1, float n2)
    {
        // Convert angle of incidence to radians
        float angleOfIncidenceRad = angleOfIncidence * Mathf.Deg2Rad;

        // Apply Snell's Law
        float sinRefractionAngle = (n1 / n2) * Mathf.Sin(angleOfIncidenceRad);

        // Check if the value is within valid range for arcsin
        if (sinRefractionAngle > 1.0f)
        {
            Debug.LogWarning("Total internal reflection occurs. No refraction.");
            return 0f; // Return 0 if total internal reflection occurs
        }

        // Calculate the refraction angle in radians and convert to degrees
        float refractionAngle = Mathf.Asin(sinRefractionAngle) * Mathf.Rad2Deg;

        return refractionAngle;
    }

    float CalculateDistance(Vector3 positionA, Vector3 positionB)
    {
        return Vector3.Distance(positionA, positionB);
    }
}


