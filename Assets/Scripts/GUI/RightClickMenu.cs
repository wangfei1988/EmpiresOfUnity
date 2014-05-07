using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RightClickMenu : MonoBehaviour {

    public static GUIScript mainGUI;
    private static GameObject UnitObject;
    private static UnitScript Unit;
    new public Camera camera;
    public GUITexture Pannel;
    //bool hold = false;
    public float ScaleX, ScaleY;
    public Rect view;
    public static bool showGUI = false;
    public static bool showSIDEgui
    {
        get
        {
            if ((Focus.masterGameObject) && (Focus.masterGameObject.GetComponent<Focus>()))
            {
                if (showGUI == false)
                    Unit = Focus.masterGameObject.GetComponent<UnitScript>();
                return true;
            }
            else return false;
        }
    }
    private static Vector2 UnitPosition;

    public GUIContent guiPannel;
    public GUIStyle buttonStyle;
    public GUIStyle guiStyle;

    public GUIStyle guiSIDEstyle;
    public GUIStyle buttonSIDEstyle;
    public Texture2D dieGruehnePower;

    void Awake()
    {
        mainGUI = GameObject.FindGameObjectWithTag("MainGUI").GetComponent<GUIScript>();
        ScaleX = camera.pixelRect.width / gameObject.guiTexture.texture.width;
        ScaleY = camera.pixelRect.height / gameObject.guiTexture.texture.height;
    }

	void Start ()
    {
        view = camera.pixelRect;

        buttonSIDEstyle.fontSize = buttonStyle.fontSize = (int)((float)buttonStyle.fontSize * ScaleX);
        guiStyle.fontSize = (int)((float)guiStyle.fontSize * ScaleX);
        guiSIDEstyle.fontSize = guiStyle.fontSize;
        guiStyle.padding.top = (int)(-64*ScaleX);
        buttonSIDEstyle.fixedWidth *= ScaleX;
        buttonSIDEstyle.fixedHeight *= ScaleY;

        UnitPosition = new Vector2(GUIScript.ScreenSize.x - 100, 130);
	}

    public static void PopUpGUI(UnitScript forUnit)
    {
        if (forUnit.gameObject.GetInstanceID() != Focus.masterGameObject.GetInstanceID())
            forUnit.gameObject.AddComponent<Focus>();

        Unit = forUnit;
        UnitPosition = MouseEvents.State.Position;
        showGUI = true;


    }

    void OnGUI()
    {
        if (showSIDEgui)
        {                          //            --------------------------------------Menü  an der Seite...
            Object[] SIDEmenuOptions = Unit.Options.GetUnitsSIDEMenuObjects();
            float btnHeight = (40 * ScaleY);
            float zwischenbuttonraum = (20 * ScaleY);
            Rect guiposition;
            guiposition = new Rect(1718 * ScaleX, (210 * ScaleY) - 3 * guiStyle.fontSize, 202 * ScaleX, 360 * ScaleY);
            GUI.BeginGroup(guiposition, Unit.name+"'s Objects:", guiSIDEstyle);
            for (int i = 0; i < SIDEmenuOptions.Length; i++)
            {               
                if (GUI.Button(new Rect(0, 3 * guiStyle.fontSize + i * (btnHeight + zwischenbuttonraum), (180 * ScaleX), btnHeight), SIDEmenuOptions[i].name))
                {
                    Unit.Options.SetSIDEObject(SIDEmenuOptions[i]);
                }
            }
            if (GUI.Button(new Rect(0, 3 * guiStyle.fontSize + SIDEmenuOptions.Length * (btnHeight + zwischenbuttonraum), (180 * ScaleX), btnHeight), "Cancel..."))
            {
                if (!Focus.IsLocked)
                    Component.Destroy(Focus.masterGameObject.GetComponent<Focus>());
            }
            GUI.EndGroup();
        }

        if (showGUI)
        {                    //--------------------------------PopUp Menu...
            float btnHeight = (40 * ScaleY);
       //     string[] menuOptions = Unit.RightClickMenuOptions;
            EnumProvider.ORDERSLIST[] selected = new EnumProvider.ORDERSLIST[1];
            EnumProvider.ORDERSLIST[] options = Unit.RightClickMenuOptionStates;
            Rect guiposition = new Rect(UnitPosition.x, view.height - UnitPosition.y, Pannel.texture.width * ScaleX, (options.Length + 1) * btnHeight + guiStyle.fontSize);
            GUI.BeginGroup(guiposition, "Orders for:\n " + Unit.name, guiStyle);
            for (int i = 0; i < options.Length; i++)
            {
                if (GUI.Button(new Rect(0, guiStyle.fontSize + i * btnHeight, Pannel.texture.width * ScaleX, btnHeight), options[i].ToString(), buttonStyle))
                {
                    Unit.Options.GiveOrder(options[i]);
                    showGUI = false;
                }
            }
            if (GUI.Button(new Rect(0, guiStyle.fontSize + options.Length * btnHeight, Pannel.texture.width * ScaleX, btnHeight), "Cancel...", buttonStyle))
            {
                showGUI = false;
            }
            GUI.EndGroup();
        }
    }

	public void DoUpdate() 
    {

	}
}
