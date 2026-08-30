using JetBrains.Annotations;
using SpiceSharp.Algebra;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MicroscopicView : MonoBehaviour
{
    [SerializeField] GameObject cross;
    [SerializeField] GameObject slab;
    [SerializeField] GameObject powder;

    public void crossView()
    {
        cross.SetActive(true);
        slab.SetActive(false);
        powder.SetActive(false);
    }

    public void slabView()
    {
        cross.SetActive(false);
        slab.SetActive(true);
        powder.SetActive(false);
    }

    public void powderView()
    {
        cross.SetActive(false);
        slab.SetActive(false);
        powder.SetActive(true);
    }
}
