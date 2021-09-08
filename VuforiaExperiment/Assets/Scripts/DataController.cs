using Fading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DataController : MonoBehaviour
{
    #region JSON Serialization data
    [Serializable]
    public class HDSystem
    {
        public bool isFraud;
        public string domain;
        public long timestamp;
    }

    [Serializable]
    public class HDComponent
    {
        public string title;
        public string value;
    }

    [Serializable]
    public class HololensData
    {
        public HDSystem system;
        public List<HDComponent> components;
    }
    #endregion

    #region Data parameters
    [SerializeField] private Color successColor = new Color(0.14f, 0.86f, 0.55f);
    [SerializeField] private Color errorColor = Color.red;
    #endregion

    #region Data fields (left panel)
    [SerializeField] private GameObject successPanel;
    [SerializeField] private Text domainText;
    [SerializeField] private Text timestampText;
    [SerializeField] private Text fraudText;

    [SerializeField] private GameObject errorPanel;
    [SerializeField] private Text errorText;
    #endregion

    #region Data fields (right panel)
    [SerializeField] private GameObject componentsParent;
    [SerializeField] private GameObject componentPrefab;
    [SerializeField] private float componentsOffset = 0.04f;
    [SerializeField] private FaderController faderController;
    #endregion

    [SerializeField] private string webDataUrl = "https://us-central1-bondroom.cloudfunctions.net/hololens";
    [SerializeField] private float fetchDataEverySeconds = 10.0f;

    private string _data;

    private bool dataFetchFailed = false;
    private float lastDataTry = 0.0f;

    private void Start()
    {
        successPanel.SetActive(false);
        errorPanel.SetActive(false);
        StartCoroutine(GetRequest(webDataUrl));
    }

    private void Update()
    {
        if (dataFetchFailed)
        {
            if (lastDataTry > fetchDataEverySeconds)
            {
                lastDataTry = 0.0f;
                dataFetchFailed = false;
                StartCoroutine(GetRequest(webDataUrl));
            } else
            {
                lastDataTry += Time.deltaTime;
            }
        }
    }

    public IEnumerator GetRequest(string uri)
    {
        bool success = false;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    _data = "Connection Error... :(";
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    _data = "Data Processing Error... :(";
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    _data = "Protocol Error... :(";
                    break;
                case UnityWebRequest.Result.Success:
                    _data = webRequest.downloadHandler.text;
                    success = true;
                    System.IO.File.WriteAllText(Application.persistentDataPath + "/Hololens.json", _data);
                    break;
            }
        }

        if (!success)
        {
            // Try to read data that was previously saved in a file
            try
            {
                _data = System.IO.File.ReadAllText(Application.persistentDataPath + "/Hololens.json");
            } 
            catch(Exception)
            {
                DisplayError();
                dataFetchFailed = true;
                yield break;
            }
        }

        dataFetchFailed = false;
        DisplayData();
    }

    private void DisplayData()
    {
        // Turn on/off appropriate panel
        successPanel.SetActive(true);
        errorPanel.SetActive(false);

        // Deserialize data
        var data = JsonUtility.FromJson<HololensData>(_data);

        CustomizeLeftPanel(data);
        CustomizeRightPanel(data);
    }

    private void CustomizeLeftPanel(HololensData data)
    {
        // Choose text color based on the system flags
        Color dataColor = data.system.isFraud ? errorColor : successColor;
        domainText.GetComponent<Fader>().Color = dataColor;
        domainText.color = dataColor;
        timestampText.GetComponent<Fader>().Color = dataColor;
        timestampText.color = dataColor;
        fraudText.GetComponent<Fader>().Color = dataColor;
        fraudText.color = dataColor;

        // Display data
        domainText.text = $"Domain: {data.system.domain}";
        timestampText.text = $"Timestamp: {UnixTimeStampToDateTime(data.system.timestamp):yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss}";
        fraudText.text = data.system.isFraud ? "FRAUD!!!" : "NOT FRAUD!";
    }

    private void CustomizeRightPanel(HololensData data)
    {
        for (int i = 0; i < data.components.Count; ++i)
        {
            // Instantiate row for every component
            var component = Instantiate(componentPrefab, componentPrefab.transform.position, componentPrefab.transform.rotation);
            component.transform.SetParent(componentsParent.transform, false);

            // Get the child components
            var titleChild = component.transform.GetChild(0);
            var valueChild = component.transform.GetChild(1);

            // Fill out the values
            titleChild.GetComponent<Text>().text = data.components[i].title;
            valueChild.GetComponent<Text>().text = data.components[i].value;

            // Offset the row
            component.transform.GetComponent<RectTransform>().anchoredPosition -= componentsOffset * i * Vector2.up;

            // Add text fields to fader controller
            faderController.AddFader(titleChild.GetComponent<Fader>());
            faderController.AddFader(valueChild.GetComponent<Fader>());
        }
    }

    private void DisplayError()
    {
        // Turn on/off appropriate panel
        successPanel.SetActive(false);
        errorPanel.SetActive(true);

        // Display data
        errorText.text = _data;
    }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is miliseconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}
