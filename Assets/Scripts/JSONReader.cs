using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{

    public TextAsset textJSON;
    
    [System.Serializable]

    public class Lab {
        public string labTitle;
    }

    [System.Serializable]

    public class LabList {
        public Lab[] Labs;
    }

    public LabList labs = new LabList();
    // Start is called before the first frame update
    void Start()
    {
        labs = JsonUtility.FromJson<LabList>(textJSON.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
