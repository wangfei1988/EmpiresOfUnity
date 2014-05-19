using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour 
{
    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 5, Screen.width / 3, Screen.height / 5), "Credits"))
        {
        }
        if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 5 * 2, Screen.width / 3, Screen.height / 5), "Developer Homepage"))
        {
            Application.OpenURL("http://media.giphy.com/media/gU25raLP4pUu4/giphy.gif");
        }
        if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 5 * 3, Screen.width / 3, Screen.height / 5), "Back"))
        {
            Application.LoadLevel("MainMenu");
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.LoadLevel("MainMenu");
        }
    }
}
