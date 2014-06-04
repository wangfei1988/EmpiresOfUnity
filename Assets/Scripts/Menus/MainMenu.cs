using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public GameObject Controls;
    public GameObject Credits;

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
        if (GUI.Button(new Rect(left, top + topPlus * 2, width, height), "Controls"))
        {
            Credits.renderer.enabled = false;
            if (Controls.renderer.isVisible)
                Controls.renderer.enabled = false;
            else
                Controls.renderer.enabled = true;
        }
        if (GUI.Button(new Rect(left, top + topPlus * 3, width, height), "Credits"))
        {
            Controls.renderer.enabled = false;
            if (Credits.renderer.isVisible)
                Credits.renderer.enabled = false;
            else
                Credits.renderer.enabled = true;
        }
        if (GUI.Button(new Rect(left, top + topPlus * 4, width, height), "Settings"))
        {
            Application.LoadLevel("Settings");
        }
        if (GUI.Button(new Rect(left, top + topPlus * 5, width, height), "Exit"))
        {
            Application.Quit();
        }
    }

}
