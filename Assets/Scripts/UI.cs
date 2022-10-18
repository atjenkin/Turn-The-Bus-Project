using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public TextAsset textJSON;
    public GameObject Button;
    public Button labButton;
    
    [System.Serializable]
    public class Lab {
        public string labTitle;
        public string labScene;
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
        foreach (Lab lab in labs.Labs) {
            // GameObject btn = (GameObject)Instantiate(Button);
            Button btn = labButton.GetComponent<Button>();
            // btn.onClick.AddListener(loadScene(lab.labScene));
        }
    }

    void loadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
