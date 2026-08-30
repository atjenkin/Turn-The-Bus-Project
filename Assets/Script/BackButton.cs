using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class BackButton : MonoBehaviour
    {
        public void BackClick()
        {
            //SceneManager.LoadScene("ExperimentNavigation");
            Debug.Log("🔙 Forcing reload of ExperimentNavigation scene");
            SceneManager.LoadScene("ExperimentNavigation", LoadSceneMode.Single);
        }
    }
