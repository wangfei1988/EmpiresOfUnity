using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
  

    void OnGUI()
    {
         if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 5, Screen.width / 3, Screen.height / 5), "Start"))
         {
             Application.LoadLevel("InGame");
         }
         if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 5 * 2, Screen.width / 3, Screen.height / 5), "Settings"))
         {
             Application.LoadLevel("Settings");
         }
         if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 5 * 3, Screen.width / 3, Screen.height / 5), "Exit"))
         {
             Application.Quit();
         }
    }

}
