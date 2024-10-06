using UnityEngine;
using UnityEngine.UI;

public class InputFieldHandler : MonoBehaviour
{
    [SerializeField] public InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        // Check if the input field component is attached
        if (inputField == null)
        {
            inputField = GetComponent<InputField>();
        }

        // Add a listener to the input field to handle value change
        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    // This method will be called whenever the input field's value changes
    void OnInputValueChanged(string inputValue)
    {
        Debug.Log("Input Field Value Changed: " + inputValue);
        // Add any additional logic to handle the input value here
    }
}
