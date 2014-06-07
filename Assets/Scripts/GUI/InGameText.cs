using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGameText : MonoBehaviour 
{
    void Start()
    {
        UpdateManager.OnUpdate+=UpdateManager_OnUpdate;
    }
    private static List<string> StaticTextLines = new List<string>();
    public int NumberOfLinesShown=4;
    public static void AddTextLine(string line)
    {
        StaticTextLines.Add(line);
    }

    private static bool _showInfo = false;
    public static bool ShowInfo
    {
        get 
        {
            return (ShowDebugText) ? ShowDebugText : _showInfo;
        }
        set
        {
            if (value!=_showInfo)
            {
                _showInfo = value;
                if(!value)
                    GUIScript.main.guiText.text="";
            }
        }
    }
    public static bool ShowDebugText = false;


    private string TextUpdate()
    {
        while (StaticTextLines.Count>=NumberOfLinesShown)
            StaticTextLines.RemoveAt(0);

        string textField = "";
        for (int i = 0;i < StaticTextLines.Count;i++)
        {
            textField += ("\n" + StaticTextLines[i]);
        }
        return textField;
    }

    void UpdateManager_OnUpdate()
    {
        if (ShowDebugText)
            guiText.text=TextUpdate();
    }
}
