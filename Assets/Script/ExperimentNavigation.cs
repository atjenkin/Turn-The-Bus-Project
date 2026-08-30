using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking; // Required for UnityWebRequest
using System.Web; // Needed for query parsing

 
public class ExperimentNavigation : MonoBehaviour
{
    public TextAsset manualJSON;
    public Button prefeb_labButton;
    public Transform parent;
    private string deepLinkExperimentId = null;
    private string lastHandledUrl = null;
    private static bool hasHandledDeepLink = false;




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

void Start()
{
    // Hook deep link events
    Application.deepLinkActivated += OnDeepLinkActivated;

    // Cold start
    if (!string.IsNullOrEmpty(Application.absoluteURL) && !hasHandledDeepLink)
    {
        Debug.Log("🌐 Handling cold-start deep link: " + Application.absoluteURL);
        OnDeepLinkActivated(Application.absoluteURL);
    }

    // Load experiment buttons from JSON
    Experiments = JsonUtility.FromJson<ExperimentList>(manualJSON.text);
    TryLaunchDeepLinkedExperiment();

    float height = -180f;
    float width = -659f;

    foreach (Experiment experiment in Experiments.Experiments)
    {
        Button btn = Instantiate(prefeb_labButton);
        btn.transform.SetParent(parent, false);
        btn.transform.position = new Vector3(width, height, 0);
        btn.GetComponent<RectTransform>().sizeDelta = new Vector2(327.8f, 48.2f);
        width += 450f;

        TextMeshProUGUI buttonText = btn.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI buttonDescription = btn.transform.Find("Text (TMP) (1)").GetComponentInChildren<TextMeshProUGUI>();
        RawImage buttonImg = btn.transform.GetChild(2).GetComponent<RawImage>();

        try
        {
            var captured = experiment;  // Required to close over the right experiment
            btn.onClick.AddListener(() => loadScene(captured.ExperimentType, captured.ExperimentJSON));
            buttonText.text = "Experiment " + experiment.ExperimentNumber;
            buttonText.fontSize = 24;
            buttonDescription.text = experiment.ExperimentTitle;
            buttonDescription.fontSize = 24;

            StartCoroutine(LoadImage(experiment.ExperimentPic, buttonImg));
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading experiment button: {ex.Message}");
        }
    }

    // Trigger deep link loading if applicable
    TryLaunchDeepLinkedExperiment();  // will internally check if ID is set

    
}


    // Start is called before the first frame update
    /*
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

*/
 
    void loadScene(string sceneName, string json)
    {
        if(sceneName == "ElectricalLab") { Circuit.expJSON = json; }
        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
   void Update()
{
    // Workaround: detect new deep links even when Unity doesn't trigger the event
    if (!string.IsNullOrEmpty(Application.absoluteURL) && Application.absoluteURL != lastHandledUrl)
    {
        Debug.Log("🔍 Manually detecting deep link from absoluteURL: " + Application.absoluteURL);
        lastHandledUrl = Application.absoluteURL;
        OnDeepLinkActivated(Application.absoluteURL);
    }
}


    void OnDeepLinkActivated(string url)
{
    Debug.Log("🔗 Deep link received: " + url);
    if (hasHandledDeepLink)
    {
        Debug.Log("⏩ Deep link already handled, ignoring: " + url);
        return;
    }

    hasHandledDeepLink = true;
    try
    {
        Uri uri = new Uri(url);
        var query = HttpUtility.ParseQueryString(uri.Query);
        deepLinkExperimentId = query["id"];
        Debug.Log("🔗 Received deep link for experiment ID: " + deepLinkExperimentId);

        TryLaunchDeepLinkedExperiment();
    }
    catch (Exception e)
    {
        Debug.LogError("Failed to parse deep link: " + e);
    }
}

 void TryLaunchDeepLinkedExperiment()
{
    if (string.IsNullOrEmpty(deepLinkExperimentId) || Experiments == null)
        return;

    foreach (Experiment exp in Experiments.Experiments)
    {
        if (exp.ExperimentNumber.ToString() == deepLinkExperimentId)
        {
            Debug.Log($"🚀 Launching experiment from deep link: {exp.ExperimentTitle}");
            loadScene(exp.ExperimentType, exp.ExperimentJSON);

            // Clear it so it doesn't keep re-triggering
            deepLinkExperimentId = null;
            return;
        }
    }

    Debug.LogWarning("No experiment found for ID: " + deepLinkExperimentId);
}

}
