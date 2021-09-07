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

    [SerializeField] private Text text;
    [SerializeField] private string webDataUrl = "https://us-central1-bondroom.cloudfunctions.net/hololens";

    private string _data;

    private void Start()
    {
        StartCoroutine(GetRequest(webDataUrl));
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
                    var data = JsonUtility.FromJson<HololensData>(_data);
                    System.IO.File.WriteAllText(Application.persistentDataPath + "/Hololens.json", _data);
                    break;
            }
        }

        if (!success)
        {
            // Try to read data that was previously saved in a file
            _data = System.IO.File.ReadAllText(Application.persistentDataPath + "/Hololens.json");
        }

        text.text = _data;
    }
}
