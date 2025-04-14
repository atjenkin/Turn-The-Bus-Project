using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    void Start()
    {
        // Get the Button component and add a listener to call ExitApp when clicked
        GetComponent<Button>().onClick.AddListener(ExitApp);
    }

    public void ExitApp()
    {
        Application.Quit(); // Quits the application
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in Editor
        #endif
    }
}
