using UnityEngine;

public class lr_Testing : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private lr_LineController line;

    private void Start()
    {
        Debug.Log("line godo"+line);
        Debug.Log("points godo"+points[0]+points[1]);
        line.SetUpLine(points);
        
    }

    private void Update()
    {
        // Update the line positions in every frame
        line.UpdateLine(points);
    }
}
