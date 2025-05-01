// // // // // // using UnityEngine;
// // // // // // using UnityEngine.UI;
// // // // // // using TMPro;  // Add this to access TextMeshPro classes


// // // // // // public class AngleController : MonoBehaviour
// // // // // // {
// // // // // //     // Reference to the top and bottom objects
// // // // // //     public GameObject topObject; // Assign your top object here in the Inspector
// // // // // //     public GameObject bottomObject; // Assign your bottom object here in the Inspector
// // // // // // public TextMeshProUGUI angleText;  
// // // // // //     void Update()
// // // // // //     {
// // // // // //         // Get positions of both objects
// // // // // //         Vector3 topPosition = topObject.transform.position;
// // // // // //         Vector3 bottomPosition = bottomObject.transform.position;

// // // // // //         // Calculate the direction from topObject to bottomObject
// // // // // //         Vector3 direction = bottomPosition - topPosition;

// // // // // //         // Define the vertical axis
// // // // // //         Vector3 verticalAxis = Vector3.up;

// // // // // //         // Calculate the angle between the direction and the vertical axis
// // // // // //         float angle = Vector3.Angle(direction, verticalAxis) -90;

// // // // // //         // Log the angle in the console
// // // // // //         Debug.Log("Angle with the vertical axis: " + angle);

// // // // // //         angleText.text = "Angle of Inc: " + angle.ToString("F2") + "°";
// // // // // //     }
// // // // // // }
// // // // // // using UnityEngine;
// // // // // // using UnityEngine.UI;
// // // // // // using TMPro;  // Access TextMeshPro classes
// // // // // // using System;  // To use Mathf functions

// // // // // // public class AngleController : MonoBehaviour
// // // // // // {
// // // // // //     public GameObject topObject; // Assign your top object here in the Inspector
// // // // // //     public GameObject bottomObject; // Assign your bottom object here in the Inspector
// // // // // //     public TextMeshProUGUI angleText; // Assign your TMP text UI element
// // // // // //     public Slider angleSlider; // Assign the slider that controls the incident angle
// // // // // //     public float refractiveIndex = 1.60f; // Set the refractive index of the material

// // // // // //     void Update()
// // // // // //     {
// // // // // //         // Update the incident angle based on the slider's value
// // // // // //         float incidentAngle = angleSlider.value;

// // // // // //         // Calculate the refractive angle using Snell's Law
// // // // // //         float refractiveAngle = CalculateRefractiveAngle(incidentAngle, refractiveIndex);

// // // // // //         // Update the angle display text
// // // // // //         angleText.text = "Incident Angle: " + incidentAngle.ToString("F2") + "°, Refractive Angle: " + refractiveAngle.ToString("F2") + "°";

// // // // // //         // Log the refractive angle in the console
// // // // // //         Debug.Log("Refractive Angle: sss" + refractiveAngle);
// // // // // //     }

// // // // // //     // Method to calculate the refractive angle using Snell's Law
// // // // // //     float CalculateRefractiveAngle(float incidentAngle, float n)
// // // // // //     {
// // // // // //         float incidentRadians = incidentAngle * Mathf.Deg2Rad; // Convert degrees to radians
// // // // // //         float sineOfRefractiveAngle = Mathf.Sin(incidentRadians) / n;
// // // // // //         Debug.Log("incidentRadians Angle: " + incidentRadians);
// // // // // //                 Debug.Log("sineOfRefractiveAngle Angle: " + sineOfRefractiveAngle);

// // // // // //         // Clamping the sine value between -1 and 1 to avoid invalid input to Asin function due to floating point precision issues
// // // // // //         sineOfRefractiveAngle = Mathf.Clamp(sineOfRefractiveAngle, -1f, 1f);
// // // // // //         float refractiveRadians = Mathf.Asin(sineOfRefractiveAngle); // Calculate the arc sine of the sine value
// // // // // //         return refractiveRadians * Mathf.Rad2Deg; // Convert radians back to degrees
// // // // // //     }
// // // // // // }
// // // // // using UnityEngine;
// // // // // using UnityEngine.UI;
// // // // // using TMPro;  // Access TextMeshPro classes
// // // // // using System;  // To use Mathf functions

// // // // // public class AngleController : MonoBehaviour
// // // // // {
// // // // //     public GameObject topObject; // Assign your top object here in the Inspector
// // // // //     public GameObject bottomObject; // Assign your bottom object here in the Inspector
// // // // //     public TextMeshProUGUI angleText; // Assign your TMP text UI element
// // // // //     public Slider angleSlider; // Assign the slider that controls the incident angle
// // // // //     public float refractiveIndex = 1.60f; // Set the refractive index of the material

// // // // //     void Start() {
// // // // //         // Initialize the slider
// // // // //         angleSlider.minValue = 0; // Corresponds to 30 degrees
// // // // //         angleSlider.maxValue = 1; // Corresponds to 60 degrees
// // // // //         angleSlider.value = MapAngleToSlider(45); // Default to 45 degrees as an example
// // // // //     }

// // // // //     void Update()
// // // // //     {
// // // // //         // Get the incident angle from the slider's value
// // // // //         float incidentAngle = MapSliderToAngle(angleSlider.value);

// // // // //         // Calculate the refractive angle using Snell's Law
// // // // //         float refractiveAngle = CalculateRefractiveAngle(incidentAngle, refractiveIndex);

// // // // //         // Update the angle display text
// // // // //         angleText.text = "Incident Angle: " + incidentAngle.ToString("F2") + "°, Refractive Angle: " + refractiveAngle.ToString("F2") + "°";

// // // // //         // Log the refractive angle in the console
// // // // //         Debug.Log("Refractive Angless: " + refractiveAngle);
// // // // //     }

// // // // //     // Convert slider value to angle
// // // // //     float MapSliderToAngle(float sliderValue) {
// // // // //         return Mathf.Lerp(30, 60, sliderValue); // Linearly maps 0 to 1 to 30 to 60 degrees
// // // // //     }

// // // // //     // Optional: map an angle back to the slider position
// // // // //     float MapAngleToSlider(float angle) {
// // // // //         return (angle - 30) / (60 - 30);
// // // // //     }

// // // // //     // Method to calculate the refractive angle using Snell's Law
// // // // //     float CalculateRefractiveAngle(float incidentAngle, float n)
// // // // //     {
// // // // //         float incidentRadians = incidentAngle * Mathf.Deg2Rad; // Convert degrees to radians
// // // // //         float sineOfRefractiveAngle = Mathf.Sin(incidentRadians) / n;
// // // // //         sineOfRefractiveAngle = Mathf.Clamp(sineOfRefractiveAngle, -1f, 1f);
// // // // //         float refractiveRadians = Mathf.Asin(sineOfRefractiveAngle); // Calculate the arc sine of the sine value
// // // // //         return refractiveRadians * Mathf.Rad2Deg; // Convert radians back to degrees
// // // // //     }
// // // // // }

// // // using UnityEngine;
// // // using UnityEngine.UI;
// // // using TMPro;

// // // public class AngleController : MonoBehaviour
// // // {
// // //     // Reference to the top and bottom objects
// // //     public GameObject topObject; // Assign your top object here in the Inspector
// // //     public GameObject bottomObject; // Assign your bottom object here in the Inspector
// // //     public GameObject thirdObject; // The third object whose position depends on the doubled angle
// // //     public Slider angleSlider; // Slider to reflect the doubled angle
// // //     public TextMeshProUGUI angleText;

// // //     void Update()
// // //     {
// // //         // Get positions of both objects
// // //         Vector3 topPosition = topObject.transform.position;
// // //         Vector3 bottomPosition = bottomObject.transform.position;

// // //         // Calculate the direction from topObject to bottomObject
// // //         Vector3 direction = bottomPosition - topPosition;

// // //         // Define the vertical axis
// // //         Vector3 verticalAxis = Vector3.up;

// // //         // Calculate the angle between the direction and the vertical axis
// // //         float angle = Vector3.Angle(direction, verticalAxis) - 90;

// // //         // Double the angle for the slider and third object
// // //         float doubledAngle = angle * 2;

// // //         // Update the angle text
// // //         angleText.text = "Angle of Inc: " + angle.ToString("F2") + "°";

// // //         // Set the slider's value to the doubled angle
// // //         angleSlider.value = angle/10;

// // //         // Position the third object based on the doubled angle (example: moving along the y-axis)
// // //         thirdObject.transform.position = bottomPosition + new Vector3(0, Mathf.Sin(doubledAngle * Mathf.Deg2Rad), Mathf.Cos(doubledAngle * Mathf.Deg2Rad));

// // //         // Log the doubled angle for verification
// // //         Debug.Log("Doubled angle: " + doubledAngle);
// // //     }
// // // }


// // using UnityEngine;
// // using UnityEngine.UI;
// // using TMPro;

// // public class AngleController : MonoBehaviour
// // {
// //     // Reference to the top and bottom objects
// //     public GameObject topObject; // Assign your top object here in the Inspector
// //     public GameObject bottomObject; // Assign your bottom object here in the Inspector
// //     public GameObject thirdObject; // The third object whose position depends on the tripled angle
// //     public Slider angleSlider; // Slider to reflect the tripled angle
// //     public TextMeshProUGUI angleText;

// //     void Update()
// //     {
// //         if (topObject == null || bottomObject == null || thirdObject == null || angleSlider == null || angleText == null)
// //     {
// //         Debug.LogError("One or more required objects are not assigned in the Inspector.");
// //         return;
// //     }   
// //         // Get positions of both objects
// //         Vector3 topPosition = topObject.transform.position;
// //         Vector3 bottomPosition = bottomObject.transform.position;
// //         Debug.Log( bottomObject.transform.position);
// //         Debug.Log( thirdObject.transform.position);

// //         // Calculate the direction from topObject to bottomObject
// //         Vector3 direction = bottomPosition - topPosition;

// //         // Define the vertical axis
// //         Vector3 verticalAxis = Vector3.up;

// //         // Calculate the angle between the direction and the vertical axis
// //         float angle = Vector3.Angle(direction, verticalAxis) - 90;
// //          float n1 = 1.0f; // Refractive index of air
// //         float n2 = 1.5f; // Refractive index of the prism materia
// //         // Call the function to get the tripled angle
// //         float tripledAngle = CalculateRefractionAngle(angle,n1,n2);

// //         // Update the angle text
// //         angleText.text = "Angle of Inc: " + angle.ToString("F2") + "°";

// //         // Set the slider's value to the tripled angle
// //         angleSlider.value = angle / 10;

// //         // Position the third object based on the tripled angle (example: moving along the y-axis)
// //         thirdObject.transform.position = bottomPosition + new Vector3(0, Mathf.Sin(tripledAngle * Mathf.Deg2Rad), Mathf.Cos(tripledAngle * Mathf.Deg2Rad));

// //         // Log the tripled angle for verification
// //         Debug.Log("Tripled angle: " + tripledAngle);
// //     }

// //     // Function to calculate the tripled angle
// //     // float CalculateTripleAngle(float angle)
// //     // {
// //     //     return angle * 3;
// //     // }
// //     float CalculateRefractionAngle(float angleOfIncidence, float n1, float n2)
// //     {
// //         // Convert angle of incidence to radians
// //         float angleOfIncidenceRad = angleOfIncidence * Mathf.Deg2Rad;

// //         // Apply Snell's Law
// //         float sinRefractionAngle = (n1 / n2) * Mathf.Sin(angleOfIncidenceRad);

// //         // Check if the value is within valid range for arcsin
// //         if (sinRefractionAngle > 1.0f)
// //         {
// //             Debug.LogWarning("Total internal reflection occurs. No refraction.");
// //             return 0f; // Return 0 if total internal reflection occurs
// //         }

// //         // Calculate the refraction angle in radians and convert to degrees
// //         float refractionAngle = Mathf.Asin(sinRefractionAngle) * Mathf.Rad2Deg;

// //         return refractionAngle;
// //     }
// // }

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AngleController : MonoBehaviour
{
    // Reference to the top and bottom objects
    public GameObject topObject; // Assign your top object here in the Inspector
    public GameObject bottomObject; //The third object whose position depends on the tripled angle
    public GameObject differentsliderObject; // The third object whose position depends on the tripled angle
    public GameObject normallineObject1;
    public GameObject normallineObject2;
    public Slider angleSlider; // Slider to reflect the tripled angle
    public TextMeshProUGUI angleText;
    public GameObject printObject; //The third object whose position depends on the tripled angle
    public GameObject printObject1; //The third object whose position depends on the tripled angle


    void Update()
    {
        if (topObject == null || bottomObject == null || angleSlider == null || angleText == null)
        {
            Debug.LogError("One or more required objects are not assigned in the Inspector.");
            return;
        }

        // Get positions of both objects
        Vector3 topPosition = topObject.transform.position;
        Vector3 bottomPosition = bottomObject.transform.position; // Added to obtain the third object's position
        Vector3 differentsliderPosition = differentsliderObject.transform.position;
        Vector3 normallinePosition1 = normallineObject1.transform.position;
        Vector3 normallinePosition2 = normallineObject2.transform.position;
        Debug.Log("printtttttttttttttttttt"+printObject.transform.position);
        Debug.Log("printtttttttttttttttttt"+printObject1.transform.position);

        // Log the names and positions of bottom and third objects
        // Debug.Log("Bottom Object: " + bottomObject.name + ", Position: " + bottomPosition);
        // Debug.Log("Third Object: " + thirdObject.name + ", Position: " + thirdPosition);

        // Calculate the direction from topObject to bottomObject
        Vector3 direction = bottomPosition - topPosition;
        Debug.Log(differentsliderPosition);
        // Debug.Log(angleSlider.transform.position);
        // above wont work 
        Debug.Log("pleaseeeeeeeeeeeeeeeeee");
        // Debug.Log(bottomPosition);
        Vector3 newdirection = differentsliderPosition - bottomPosition;
        Vector3 normallinePosition = normallinePosition2 - normallinePosition1;


        // Vector3 refrectiveAngle = 

        // Define the vertical axis
        Vector3 verticalAxis = Vector3.up;
        // Calculate the angle between the direction and the vertical axis
        float angle = Vector3.Angle(direction, normallinePosition) ;
        float bhide = 180 - angle;
        // angle = -angle;
        angle = angle -90;

        float refrectiveAngle = Vector3.Angle(newdirection, verticalAxis) ;
        float adjustedAngle = refrectiveAngle - 175;
       
        // float newangle = Vector3.Angle(direction, verticalAxis) - 90;
        float newangle = Vector3.Angle(newdirection, verticalAxis) - 90;

        // Debug.Log(newangle +" ======"+ newdirection+" ----------------------------");
        // Debug.Log("bottom position"+ bottomPosition + "slider position" + differentsliderPosition  +)
        float n1 = 1.0f; // Refractive index of air
        float n2 = 1.5f; // Refractive index of the prism material
        // Call the function to get the tripled angle
        float tripledAngle = CalculateRefractionAngle(angle, n1, n2);
        Debug.Log("iiiiiiiiiiiiiiiiiiiiii"+angle + "......"+bhide);
        Debug.Log("real iiiii" + newdirection);
        Debug.Log(refrectiveAngle + "....="+adjustedAngle+".....................................");
        Debug.Log("rrrrrrrrrrrrr" + tripledAngle);
        Vector3 refractedDirection = RotateRayDirection(bottomPosition, tripledAngle);
        Debug.DrawLine(bottomPosition, topPosition + normallinePosition, Color.green); // Normal line
        Debug.Log( Vector3.Angle(refractedDirection, verticalAxis) );

        // Debug.Log("salvano"+refrectiveAngle);
        // Update the angle text
        angleText.text = "Angle of Incs: " + angle.ToString("F2") + "°";
     float maxAngle = 41.81f; // Maximum value of the angle
         float maxSliderValue = 0.66f; // Maximum value of the slider

        // Set the slider's value to the tripled angle
        angleSlider.value =(tripledAngle / maxAngle) * maxSliderValue;
        // Debug.Log(differentsliderPosition);
        // Debug.Log(angleSlider.transform.position);
        // Debug.Log("pleaseeeeeeeeeeeeeeeeee");
        // Debug.Log(bottomPosition);
        // angleSlider.onValueChanged.AddListener(LogHandlePosition);
               // Access the handle's RectTransform
        RectTransform handleRect = angleSlider.handleRect;

        // Get the world position of the handle
        Vector3 handlePosition = handleRect.position;

        // Get the current slider value
        float value = angleSlider.value;

        // Log the handle position and slider value
        // Debug.Log($"Slider Value: {value}, Handle Position: {handlePosition}");

        Vector3 xdirection = handlePosition - bottomPosition;
        // Debug.Log("is this new?");
        Debug.Log(175- Vector3.Angle(xdirection, normallinePosition));
        // Position the third object based on the tripled angle (example: moving along the y-axis)
        // thirdObject.transform.position = bottomPosition + new Vector3(0, Mathf.Sin(tripledAngle * Mathf.Deg2Rad), Mathf.Cos(tripledAngle * Mathf.Deg2Rad));

        // Calculate and log the angle between the bottom and the third object
        // Vector3 directionToThird = thirdPosition - bottomPosition;
        float distance = CalculateDistance(bottomObject.transform.position, handlePosition);
        // Debug.Log("cccccccccangle slider value cccccccccccccccc   " + angleSlider.value);
        // float angleToThird = Vector3.Angle(directionToThird, direction); // Calculate the angle relative to the initial direction
        // Debug.Log("Angle between bottom and third objects: " + angleToThird + "°");
        Vector3 newPosition = CalculatePositionFromAngle(bottomPosition, tripledAngle, distance);
        // differentsliderObject.transform.position = newPosition;
        // Log the tripled angle for verification
        // Debug.Log("Tripled angle: " + bottomPosition + tripledAngle);
    }

        Vector3 RotateRayDirection(Vector3 originalDirection, float refractedAngle)
    {
        // Rotate the vector by the angle
        Quaternion rotation = Quaternion.Euler(0, 0, refractedAngle);  // Assuming you rotate around the Z-axis
        return rotation * originalDirection;
    }


    void LogHandlePosition(float value)
    {
        // Access the handle's RectTransform
        RectTransform handleRect = angleSlider.handleRect;

        // Get the world position of the handle
        Vector3 handlePosition = handleRect.position;

        // Log the handle position
        Debug.Log($"Slider Value: {value}, Handle Position: {handlePosition}");
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





// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class AngleController : MonoBehaviour
// {
//     public GameObject bottomObject; // Fixed bottom point
//     public GameObject differentsliderObject; // The object that moves along the triangle's side
//     public Slider angleSlider; // Slider to control the angle dynamically
//     public TextMeshProUGUI angleText; // Display the current angle

//     private float sideLength = 20.0f; // Approximate side length of the triangle

//     void Update()
//     {
//         // Calculate the desired refraction angle from Snell's law or similar method
//         float desiredAngle = CalculateRefractionAngle(); // Implement this function based on your specific refraction logic

//         // Update the position of differentsliderObject based on the slider value
//         UpdateDifferentsliderPosition(desiredAngle);

//         // Display the current angle dynamically
//         float currentAngle = CalculateCurrentAngle();
//         angleText.text = $"Current Angle: {currentAngle:F2}°";
//     }

//     void UpdateDifferentsliderPosition(float desiredAngle)
//     {
//         // Calculate the new position based on the desired angle
//         float distance = sideLength * angleSlider.value; // Scale the distance by the slider value
//         Vector3 newPosition = CalculatePositionFromAngle(bottomObject.transform.position, desiredAngle, distance);
//         differentsliderObject.transform.position = newPosition;
//     }

//     Vector3 CalculatePositionFromAngle(Vector3 startPoint, float angle, float distance)
//     {
//         // Convert angle to radians
//         float angleRad = angle * Mathf.Deg2Rad;

//         // Calculate new position
//         float x = Mathf.Cos(angleRad) * distance;
//         float y = Mathf.Sin(angleRad) * distance;

//         // Assuming angle is with respect to the horizontal axis
//         return new Vector3(startPoint.x + x, startPoint.y + y, startPoint.z);
//     }

//     float CalculateCurrentAngle()
//     {
//         // Calculate and return the current angle based on the positions of bottomObject and differentsliderObject
//         Vector3 direction = differentsliderObject.transform.position - bottomObject.transform.position;
//         return Vector3.Angle(direction, Vector3.up) - 90; // Adjust this calculation as needed
//     }

//     float CalculateRefractionAngle(float angleOfIncidence, float n1, float n2)
//     {
//         // Convert angle of incidence to radians
//         float angleOfIncidenceRad = angleOfIncidence * Mathf.Deg2Rad;

//         // Apply Snell's Law
//         float sinRefractionAngle = (n1 / n2) * Mathf.Sin(angleOfIncidenceRad);

//         // Check if the value is within valid range for arcsin
//         if (sinRefractionAngle > 1.0f)
//         {
//             Debug.LogWarning("Total internal reflection occurs. No refraction.");
//             return 0f; // Return 0 if total internal reflection occurs
//         }

//         // Calculate the refraction angle in radians and convert to degrees
//         float refractionAngle = Mathf.Asin(sinRefractionAngle) * Mathf.Rad2Deg;

//         return refractionAngle;
//     }
    
// }
