using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLabel : MonoBehaviour
{
    private void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = false;
    }
}
