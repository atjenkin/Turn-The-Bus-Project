using UnityEngine;

public class LensScript2D : MonoBehaviour
{
    public GameObject pencil; // Assign the pencil GameObject in the Inspector
    public float focalLength; // Set the focal length in the Inspector
    private GameObject image;

    void Start()
    {
        // Create an image of the pencil
        image = Instantiate(pencil);
        image.GetComponent<SpriteRenderer>().color = Color.blue; // Different color to distinguish

        // Calculate the image position
        UpdateImagePosition();
    }

    void Update()
    {
        // Update image position dynamically if the pencil moves
        UpdateImagePosition();
    }

    void UpdateImagePosition()
    {
        Vector3 pencilPosition = pencil.transform.position;
        Vector3 lensPosition = transform.position;

        // Calculate object distance (distance from pencil to lens along the x-axis)
        float objectDistance = pencilPosition.x - lensPosition.x;

        // Image distance using lens formula (1/f = 1/do + 1/di)
        float imageDistance = 1 / (1 / focalLength - 1 / objectDistance);

        // Position the image on the opposite side of the lens
        image.transform.position = new Vector3(lensPosition.x + imageDistance, pencilPosition.y, pencilPosition.z);

        // Invert the image along the x-axis
        image.transform.localScale = new Vector3(-pencil.transform.localScale.x, pencil.transform.localScale.y, pencil.transform.localScale.z);
    }
}

