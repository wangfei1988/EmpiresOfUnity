using UnityEngine;
using System.Collections;

public class MainOnGUIMenu : MonoBehaviour
{
    public GUIScript GuiScript;
    private Scrolling scrolling;

    void Start()
    {
        GuiScript = this.GetComponent<GUIScript>();
        scrolling = (GetComponent<Scrolling>()) ? GetComponent<Scrolling>() : null;
    }

    void OnGUI()
    {

        GUI.BeginGroup(new Rect((1718*GuiScript.Scale.x), (24*GuiScript.Scale.y), (180 * GuiScript.Scale.x), (160 * GuiScript.Scale.y)));
        if (GUI.Button(new Rect((0*GuiScript.Scale.x), (0*GuiScript.Scale.y), (180*GuiScript.Scale.x), (40*GuiScript.Scale.y)), GuiScript.MainGuiContent[0]) || Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("MainMenu");
        }

        if (GUI.Button(new Rect((0 * GuiScript.Scale.x), (60 * GuiScript.Scale.y), (80 * GuiScript.Scale.x), (40 * GuiScript.Scale.y)), GuiScript.MainGuiContent[1]))
        {
            scrolling.SwitchScrollingStatus();
        }
        if (GUI.Button(new Rect((100 * GuiScript.Scale.x), (60 * GuiScript.Scale.y), (80 * GuiScript.Scale.x), (40 * GuiScript.Scale.y)), GuiScript.MainGuiContent[2]))
        {
            //Camera.main.GetComponent<Cam>().SwitchCam();
            GUIScript.MiniMap.SwitchActive();
        }

        if (GUI.Button(new Rect((0 * GuiScript.Scale.x), (120 * GuiScript.Scale.y), (47 * GuiScript.Scale.x), (40 * GuiScript.Scale.y)), GuiScript.MainGuiContent[3]))
        {
            Ground.Switch(0);
        }
        if (GUI.Button(new Rect((68 * GuiScript.Scale.x), (120 * GuiScript.Scale.y), (47 * GuiScript.Scale.x), (40 * GuiScript.Scale.y)), GuiScript.MainGuiContent[4]))
        {
            Ground.Switch(1);
        }
        if (GUI.Button(new Rect((134 * GuiScript.Scale.x), (120 * GuiScript.Scale.y), (47 * GuiScript.Scale.x), (40 * GuiScript.Scale.y)), GuiScript.MainGuiContent[5]))
        {
            Ground.Switch(2);
        }
        GUI.enabled = true;
        GUI.EndGroup();

    }

}
