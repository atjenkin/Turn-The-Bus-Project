using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking; // Required for UnityWebRequest
 
public class ExperimentNavigation : MonoBehaviour
{
    public TextAsset manualJSON;
    public Button prefeb_labButton;
    public Transform parent;

    [System.Serializable] public class Experiment
    {
        public float ExperimentNumber;
        public string ExperimentType;
        public string ExperimentTitle;
        public string ExperimentPic;
        public string ExperimentJSON;
    }

    [System.Serializable] public class ExperimentList
    {
        public Experiment[] Experiments;
    }

    public class ButtonContent : MonoBehaviour
    {
        public TextMeshProUGUI ButtonText;
        public RawImage ButtonImage;
        public TextMeshProUGUI ButtonDescription;
    }

    public ExperimentList Experiments = new ExperimentList();

IEnumerator LoadImage(string filePath, RawImage buttonImg)
{
    string imagePath = Path.Combine(Application.streamingAssetsPath, filePath);

    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imagePath))
    {
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
            buttonImg.texture = tex;
        }
        else
        {
            Debug.LogError($"Error loading image: {uwr.error}");
            buttonImg.texture = null; // Set fallback texture
        }
    }
}

    
    // Start is called before the first frame update
  void Start()
{
    Experiments = JsonUtility.FromJson<ExperimentList>(manualJSON.text);
    float height = -180f;
    float width = -659f;

    foreach (Experiment experiment in Experiments.Experiments)
    {
        Button btn = Instantiate(prefeb_labButton); // Create the button once
        btn.transform.SetParent(parent, false);
        btn.transform.position = new Vector3(width, height, 0);
        btn.GetComponent<RectTransform>().sizeDelta = new Vector2(327.8f, 48.2f);
        width += 450f;

        TextMeshProUGUI buttonText = btn.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI buttonDescription = btn.transform.Find("Text (TMP) (1)").GetComponentInChildren<TextMeshProUGUI>();
        RawImage buttonImg = btn.transform.GetChild(2).GetComponent<RawImage>();

        try
        {
            // Set button functionality and properties
            btn.onClick.AddListener(() => loadScene(experiment.ExperimentType, experiment.ExperimentJSON));
            buttonText.text = "Experiment " + experiment.ExperimentNumber.ToString();
            buttonText.fontSize = 24;
            buttonDescription.text = experiment.ExperimentTitle;
            buttonDescription.fontSize = 24;

            // Load and set button image
            StartCoroutine(LoadImage(experiment.ExperimentPic, buttonImg));
            /*
            string imagePath = Path.Combine(Application.streamingAssetsPath, experiment.ExperimentPic);
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            //byte[] imageBytes = File.ReadAllBytes(experiment.ExperimentPic);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            buttonImg.texture = tex;
            */
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading experiment button: {ex.Message}");

            // Set fallback properties
            btn.onClick.AddListener(() => Debug.Log("Fallback button clicked")); // Replace with meaningful fallback action
            buttonText.text = "Experiment " + experiment.ExperimentNumber.ToString();
            buttonDescription.text = experiment.ExperimentTitle;
            buttonImg.texture = null; // Optionally clear or set a placeholder texture
        }
    }
}


 
    void loadScene(string sceneName, string json)
    {
        if(sceneName == "ElectricalLab") { Circuit.expJSON = json; }
        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
