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

    public static bool showCommandPannel = false;
    public static bool showObjectPannel
    {
        get
        {
            if (Focus.masterGameObject != null
                && Focus.masterGameObject.GetComponent<Focus>()
                && Focus.masterGameObject.GetComponent<UnitScript>()
            )
            {
                if ((showCommandPannel == false))
                    Unit = Focus.masterGameObject.GetComponent<UnitScript>();
                return true;
            }
            return false;
        }
    }
    //private static Vector2 UnitPosition;

    public GUIContent guiPannel;
    public GUIStyle buttonStyle;
    public GUIStyle guiStyle;

    public GUIStyle guiSIDEstyle;
    public GUIStyle buttonSIDEstyle;
    public Texture2D dieGruehnePower;
    float sideMenuHeight = 0;
    // Building Builder -> Prefabs
    //public Object[] BuildableBuildings;
    private BuildingBuilder buildingBuilder;

    void Awake()
    {
        ScaleX = camera.pixelRect.width / gameObject.guiTexture.texture.width;
        ScaleY = camera.pixelRect.height / gameObject.guiTexture.texture.height;
    }

	void Start ()
    {
        view = camera.pixelRect;
        mainGUI = GUIScript.main.GetComponent<GUIScript>();
	    buildingBuilder = GameObject.FindGameObjectWithTag("ScriptContainer").GetComponent<BuildingBuilder>();

        buttonSIDEstyle.fontSize = buttonStyle.fontSize = (int)((float)buttonStyle.fontSize * ScaleX);
        guiStyle.fontSize = (int)((float)guiStyle.fontSize * ScaleX);
        guiSIDEstyle.fontSize = (int)((float)guiSIDEstyle.fontSize * ScaleX);
        guiStyle.border.left = (int)(-1.62f * ScaleX);
        guiStyle.border.right = (int)(-3.25f * ScaleX);
        guiStyle.border.top = (int)(20.5f * ScaleY);
        guiStyle.border.bottom = (int)(20.5f * ScaleY);
        guiStyle.padding.top = (int)(-15f*ScaleX);
        guiStyle.padding.left = (int)(23.79f * ScaleX);
        guiStyle.overflow.top = (int)(-6.3f * ScaleY);
        guiStyle.overflow.bottom = (int)(9.5 * ScaleY);

        buttonSIDEstyle.fixedWidth *= ScaleX;
        buttonSIDEstyle.fixedHeight *= ScaleY;

        //UnitPosition = new Vector2(GUIScript.ScreenSize.x - 100, 130);
	}

    public static void PopUpGUI(UnitScript forUnit)
    {
        if (forUnit.gameObject.GetInstanceID() != Focus.masterGameObject.GetInstanceID())
            forUnit.gameObject.AddComponent<Focus>();

        Unit = forUnit;
        //UnitPosition = MouseEvents.State.Position;
        showCommandPannel = true;
    }

    void OnGUI()
    {
        
        if (showObjectPannel)
        {  
            // Menü  an der Seite...
            Object[] SIDEmenuOptions = Unit.SellectableObjects;
            float btnHeight = (60 * ScaleY);
         //   float zwischenbuttonraum = (20 * ScaleY);
            Rect guiposition;
            sideMenuHeight = ((btnHeight ) * (SIDEmenuOptions.Length + 2));
         //   guiposition = new Rect(1718 * ScaleX, (210 * ScaleY) - 3 * guiStyle.fontSize, 202 * ScaleX, sideMenuHeight );
            guiposition = new Rect(1695 * ScaleX, (210 * ScaleY) - 3 * guiStyle.fontSize, 225 * ScaleX, sideMenuHeight);
            GUI.BeginGroup(guiposition, "", guiSIDEstyle); // Unit.name+"'s Activities:"
            for (int i = 0; i < SIDEmenuOptions.Length; i++)
            {               
           //     if (GUI.Button(new Rect(0, 3 * guiStyle.fontSize + i * (btnHeight + zwischenbuttonraum), (180 * ScaleX), btnHeight), SIDEmenuOptions[i].name))
                if (GUI.Button(new Rect(0, 3 * guiStyle.fontSize + i * (btnHeight ), (224 * ScaleX), btnHeight), SIDEmenuOptions[i].name,buttonSIDEstyle))
                {
                    Unit.Sellect(SIDEmenuOptions[i]);
                }
            }
            if (GUI.Button(new Rect(0, 3 * guiStyle.fontSize + SIDEmenuOptions.Length * (btnHeight), (224 * ScaleX), btnHeight), "Cancel", buttonSIDEstyle))
            {
                if (!Focus.IsLocked)
                    Component.Destroy(Focus.masterGameObject.GetComponent<Focus>());
            }
            GUI.EndGroup();
        }
        else
        {
            // Side-Menu
            Object[] SIDEmenuOptions = this.buildingBuilder.BuildableBuildings;
            float btnHeight = (60 * ScaleY);
        //    float zwischenbuttonraum = (20 * ScaleY);
            Rect guiposition;
            sideMenuHeight = ((btnHeight) * (SIDEmenuOptions.Length + 1));
         //   guiposition = new Rect(1718 * ScaleX, (210 * ScaleY) - 3 * guiStyle.fontSize, 202 * ScaleX, 360 * ScaleY);
            guiposition = new Rect(1695 * ScaleX, (210 * ScaleY) - 3 * guiStyle.fontSize, 224 * ScaleX, sideMenuHeight);
            GUI.BeginGroup(guiposition, guiSIDEstyle);
            for (int i = 0; i < SIDEmenuOptions.Length; i++)
            {
                if (GUI.Button(new Rect(0, 3 * guiStyle.fontSize + i * (btnHeight), (224 * ScaleX), btnHeight), SIDEmenuOptions[i].name, buttonSIDEstyle))
                {
                    // Code for Build-Action

                    // TODO Lucas

                    this.buildingBuilder.CreatePrefab(i);
                }
            }
            GUI.EndGroup();
        }

        if (showCommandPannel)
        {

            // If Unit of RightClickMenu was destroyed -> hide Menu
            if (Unit == null)
            {
                showCommandPannel = false;
                return;
            }

            /* [Rightclick] PopUp Menu */
            float btnHeight = (40 * ScaleY);

            //string[] menuOptions = Unit.RightClickMenuOptions;
            //EnumProvider.ORDERSLIST[] selected = new EnumProvider.ORDERSLIST[1];
            EnumProvider.ORDERSLIST[] options = Unit.RightClickMenuOptionStates;
           // Rect guiposition = new Rect(1695 * ScaleX, 590 * ScaleY, 223 * ScaleX, (options.Length + 1) * btnHeight + guiStyle.fontSize);
            Rect guiposition = new Rect(1695 * ScaleX, sideMenuHeight + ((210 * ScaleY) - 2 * guiStyle.fontSize), 223 * ScaleX, (options.Length + 1) * btnHeight + guiStyle.fontSize);
          //  Rect guiposition = new Rect(UnitPosition.x, view.height - UnitPosition.y, Pannel.texture.width * ScaleX, (options.Length + 1) * btnHeight + guiStyle.fontSize);
            GUI.BeginGroup(guiposition, "Orders:", guiStyle);
            for (int i = 0; i < options.Length; i++)
            {
                if (GUI.Button(new Rect(22 *ScaleX, guiStyle.fontSize + i * btnHeight, 180 * ScaleX, btnHeight), options[i].ToString(), buttonStyle))
                {
                    Unit.Options.GiveOrder(options[i]);
                    Debug.Log("order given to unit!");
                    showCommandPannel = false;
                }
            }
            if (GUI.Button(new Rect(22 * ScaleX, guiStyle.fontSize + options.Length * btnHeight, 180 * ScaleX, btnHeight), "Cancel...", buttonStyle))
            {
                showCommandPannel = false;
            }
            GUI.EndGroup();
        }
    }

	public void DoUpdate() 
    {

	}
}
