using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
  

    void OnGUI()
    {
        Vector2 size = new Vector2(1920, 1080);

        //float left = Screen.width / 2 - Screen.width / 4;
        float left = Screen.width/size.x*350f;
        float top = Screen.height / size.y * 100f;
        float topPlus = Screen.width / size.x * 100f;
        float width = Screen.width / size.x * 400f;
        float height = Screen.width / size.x * 80f;


        if (GUI.Button(new Rect(left, top + topPlus *1, width, height), "Start"))
         {
             Application.LoadLevel("InGame");
         }
        if (GUI.Button(new Rect(left, top + topPlus * 2, width, height), "Settings"))
         {
             Application.LoadLevel("Settings");
         }
        if (GUI.Button(new Rect(left, top + topPlus * 3, width, height), "Exit"))
         {
             Application.Quit();
         }
    }

}
