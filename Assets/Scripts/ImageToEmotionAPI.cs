using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;

public class ImageToEmotionAPI : MonoBehaviour, IInputClickHandler
{

    string EMOTIONKEY = "2ef2694581074474a5e068000ae3cfac"; // replace with your Emotion API Key

    string emotionURL = "https://api.projectoxford.ai/emotion/v1.0/recognize";

    public string fileName { get; private set; }
    string responseData;

    private ShowImageOnPanel panel; //mori
    public Text text; //mori

    // Use this for initialization
    void Start () {
	    fileName = Path.Combine(Application.streamingAssetsPath, "myphoto.jpeg"); // Replace with your file

        InputManager.Instance.PushFallbackInputHandler(gameObject); //mori
        panel = gameObject.GetComponent<ShowImageOnPanel>(); //mori
    }
	
	// Update is called once per frame
	void Update () {
	
        // This will be called with your specific input mechanism
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GetEmotionFromImages());
        }

	}
    /// <summary>
    /// Get emotion data from the Cognitive Services Emotion API
    /// Stores the response into the responseData string
    /// </summary>
    /// <returns> IEnumerator - needs to be called in a Coroutine </returns>
    IEnumerator GetEmotionFromImages()
    {
        byte[] bytes = UnityEngine.Windows.File.ReadAllBytes(fileName);

        var headers = new Dictionary<string, string>() {
            { "Ocp-Apim-Subscription-Key", EMOTIONKEY },
            { "Content-Type", "application/octet-stream" }
        };

        WWW www = new WWW(emotionURL, bytes, headers);

        yield return www;
        responseData = www.text; // Save the response as JSON string
        Debug.Log(responseData);
        GetComponent<ParseEmotionResponse>().ParseJSONData(responseData);

        text.text = responseData; //mori
    }

    public void OnInputClicked(InputEventData eventData) //mori
    {
        panel.DisplayImage(); //無理やり写真を読む

        StartCoroutine(GetEmotionFromImages());

    }
}
