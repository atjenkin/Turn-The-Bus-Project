using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    
    // Start is called before the first frame update
    void Start()
    {
        Experiments = JsonUtility.FromJson<ExperimentList>(manualJSON.text);
        float height = -180f;
        float width = -659f;
        foreach(Experiment experiment in Experiments.Experiments)
        {
            Button btn = Instantiate(prefeb_labButton);
            btn.transform.SetParent(parent,false);
            btn.transform.position = new Vector3(width, height, 0);
            btn.GetComponent<RectTransform>().sizeDelta = new Vector2(327.8f, 48.2f);
            width += 450f;
            btn.onClick.AddListener(() => loadScene(experiment.ExperimentType, experiment.ExperimentJSON));
            TextMeshProUGUI buttonText = btn.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI buttondescription = btn.transform.Find("Text (TMP) (1)").GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "Experiment "+experiment.ExperimentNumber.ToString();
            buttonText.fontSize = 24;
            buttondescription.text = experiment.ExperimentTitle.ToString();
            buttondescription.fontSize = 24;
            RawImage buttonImg = btn.transform.GetChild(2).GetComponent<RawImage>();
            byte[] imageBytes = File.ReadAllBytes(experiment.ExperimentPic);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            buttonImg.texture = tex;

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
