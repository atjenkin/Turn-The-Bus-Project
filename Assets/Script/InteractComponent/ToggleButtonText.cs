// // using UnityEngine;
// // using UnityEngine.UI; // Include the UI namespace to work with UI components.
// // using TMPro; // Add this namespace to access TextMeshPro components

// // public class ToggleButtonText : MonoBehaviour
// // {
// //     // private bool isRunning = false; // To keep track of button state
// //     public Button myButton; // Reference to the button
// //     public LineRenderer lineRenderer; // Reference to the Line Renderer
// //     public LineRenderer lineRenderer1; // Reference to the Line Renderer
// //     public LineRenderer lineRenderer2; // Reference to the Line Renderer
// //     public TextMeshProUGUI buttonText; // Reference to the TextMeshPro component


// //     private bool lineVisible = false; // Track visibility of the line
// //     private bool lineVisible1 = false; // Track visibility of the line
// //     private bool lineVisible2 = false; // Track visibility of the line

// //     void Start()
// //     {
// //         buttonText = myButton.GetComponentInChildren<TextMeshProUGUI>();
        
// //         myButton.onClick.AddListener(ToggleText); // Add an event listener for the button click
// //         UpdateButtonText(); // Initial button text update
// //     }

// //     void ToggleText()
// //     {
// //         lineVisible = !lineVisible; // Toggle the visibility state
// //         lineVisible1 = !lineVisible1; // Toggle the visibility state
// //         lineVisible2 = !lineVisible2; // Toggle the visibility state
// //         buttonText.text = "Start";
// //         // buttonText.text = buttonText.text == "Start" ? "Stop" : "Start";

// //         // isRunning = !isRunning; // Toggle the state
// //         UpdateButtonText(); // Update the button text based on the current state
// //     }

// //     void UpdateButtonText()
// //     {
// //         lineRenderer.enabled = lineVisible; // Control the Line Renderer's enabled state
// //         lineRenderer1.enabled = lineVisible1; // Control the Line Renderer's enabled state
// //         lineRenderer2.enabled = lineVisible2; // Control the Line Renderer's enabled state

// //         // buttonText.text = "Start";
// //         buttonText.text = lineVisible ? "Stop" : "Start";
        
// //         // myButton.GetComponentInChildren<Text>().text = isRunning ? "Stop" : "Start";
// //     }
// // }


// using UnityEngine;
// using UnityEngine.UI;
// using TMPro; // Use if your text is a TextMeshPro component

// public class ToggleButtonAndLines : MonoBehaviour
// {
//     public Button myButton; // Reference to the button
//     public TextMeshProUGUI buttonText; // Reference to the TextMeshPro component, change to Text if using standard UI Text
//     public LineRenderer lineRenderer; // Reference to the first Line Renderer
//     public LineRenderer lineRenderer1; // Reference to the second Line Renderer
//     public LineRenderer lineRenderer2; // Reference to the third Line Renderer

//     private bool areLinesVisible = false; // Combined state for all lines

//     void Start()
//     {
//         // Set initial state and add listener
//         buttonText.text = "Start"; // Initial text
//         // lineRenderer.enabled = false;
//         myButton.onClick.AddListener(ToggleLinesAndText); // Add event listener for the button click
//     }

//     void ToggleLinesAndText()
//     {
//         // Toggle the visibility of lines and text
//         areLinesVisible = !areLinesVisible; // Toggle the combined visibility state
        
//         // Update the visibility of each line renderer
//         lineRenderer.enabled = areLinesVisible;
//         lineRenderer1.enabled = areLinesVisible;
//         lineRenderer2.enabled = areLinesVisible;

//         // Update the button text based on the visibility of the lines
//         buttonText.text = areLinesVisible ? "Stop" : "Start";
//     }
// }


// using UnityEngine;
// using UnityEngine.UI;
// using TMPro; // Use if your text is a TextMeshPro component

// public class ToggleButtonAndLines : MonoBehaviour
// {
//     public Button myButton; // Reference to the button
//     public TextMeshProUGUI buttonText; // Reference to the TextMeshPro component, change to Text if using standard UI Text
//     public LineRenderer lineRenderer; // Reference to the first Line Renderer
//     public LineRenderer lineRenderer1; // Reference to the second Line Renderer
//     public LineRenderer lineRenderer2; // Reference to the third Line Renderer

//     public TextMeshProUGUI U;
//     public TextMeshProUGUI V;
//     private bool areLinesVisible = false; // Combined state for all lines

//     void Start()
//     {
//         // Set initial state and add listener
//         buttonText.text = "Start"; // Initial text
        
//         // Set initial visibility of lines to false
//         lineRenderer.enabled = false;
//         lineRenderer1.enabled = false;
//         lineRenderer2.enabled = false;

//         myButton.onClick.AddListener(ToggleLinesAndText); // Add event listener for the button click
//     }

//     void ToggleLinesAndText()
//     {
//         // Toggle the visibility of lines and text
//         areLinesVisible = !areLinesVisible; // Toggle the combined visibility state
        
//         // Update the visibility of each line renderer
//         lineRenderer.enabled = areLinesVisible;
//         lineRenderer1.enabled = areLinesVisible;
//         lineRenderer2.enabled = areLinesVisible;

//         // Update the button text based on the visibility of the lines
//         buttonText.text = areLinesVisible ? "Stop" : "Start";
//     }
// }



using UnityEngine;
using UnityEngine.UI;
using TMPro; // Use if your text is a TextMeshPro component

public class ToggleButtonText : MonoBehaviour
{
    public Button myButton; // Reference to the button
    public TextMeshProUGUI buttonText; // Reference to the button's TextMeshPro component
    public LineRenderer lineRenderer; // Reference to the first Line Renderer
    public LineRenderer lineRenderer1; // Reference to the second Line Renderer
    public LineRenderer lineRenderer2; // Reference to the third Line Renderer
    public TextMeshProUGUI U; // Reference to the TextMeshPro component for U
    public TextMeshProUGUI V; // Reference to the TextMeshPro component for V

    private bool areLinesVisible = false; // Combined state for all lines

    void Start()
    {
        // Set initial state and add listener
        buttonText.text = "Start"; // Initial button text
        
        // Initially disable line renderers
        lineRenderer.enabled = false;
        lineRenderer1.enabled = false;
        lineRenderer2.enabled = false;

        myButton.onClick.AddListener(ToggleLinesAndText); // Add event listener for the button click
    }

    void ToggleLinesAndText()
    {
        // Toggle the visibility of lines and text
        areLinesVisible = !areLinesVisible; // Toggle the combined visibility state
        
        // Update the visibility of each line renderer
        lineRenderer.enabled = areLinesVisible;
        lineRenderer1.enabled = areLinesVisible;
        lineRenderer2.enabled = areLinesVisible;

        // Update the button text based on the visibility of the lines
        buttonText.text = areLinesVisible ? "Stop" : "Start";

        // Update the text of U and V based on the visibility of the lines
        if (!areLinesVisible)
        {
            U.text = "None";
            V.text = "None";
        }
        else
        {
            U.text = ""; // Set to whatever the default or appropriate value should be
            V.text = ""; // Set to whatever the default or appropriate value should be
        }
    }
}

