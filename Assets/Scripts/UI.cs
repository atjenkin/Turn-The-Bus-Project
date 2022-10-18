using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{

    public TextAsset textJSON;
    public Button labButton;
    public Transform parent;
    
    [System.Serializable]
    public class Lab {
        public string labTitle;
        public string labScene;
    }

    [System.Serializable]
    public class LabList {
        public Lab[] Labs;
    }

    public class ButtonContent : MonoBehaviour
    {
        public TextMeshProUGUI buttonText;
    }

    public LabList labs = new LabList();
    // Start is called before the first frame update
    void Start()
    {
        labs = JsonUtility.FromJson<LabList>(textJSON.text);
        foreach (Lab lab in labs.Labs) {
            // GameObject btn = (GameObject)Instantiate(Button);
            Button btn = Instantiate(labButton);
            btn.transform.parent = parent;
            btn.transform.position = new Vector3(0, 100, 0);
            btn.onClick.AddListener(() => loadScene(lab.labScene));
            TextMeshProUGUI buttonText = btn.GetComponentsInChildren<TextMeshProUGUI>()[0];
            buttonText.text = lab.labTitle;
            buttonText.fontSize = 6;
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
