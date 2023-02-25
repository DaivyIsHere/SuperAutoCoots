using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugHelper : PersistentSingleton<DebugHelper>
{
    public string msg;
    public GameObject panel;
    public TMP_Text text;

    void Start()
    {
        Application.logMessageReceivedThreaded += LogCallback;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
            panel.SetActive(!panel.activeSelf);
        
        text.text = msg;
    }

    private void LogCallback(string condition, string stackTrace, LogType type)
    {
        msg += condition + "        StackTrace : " + stackTrace + "\n";
    }
}
