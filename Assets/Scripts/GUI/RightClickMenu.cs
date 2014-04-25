using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RightClickMenu : MonoBehaviour {

    public static GUIScript mainGUI;
    private static GameObject UnitObject;
    private static UnitScript Unit;
    new public Camera camera;
    public GUITexture Pannel;
 //   bool hold = false;
    public float ScaleX, ScaleY;
    public Rect view;
    public static bool showGUI = false;
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
        

	}



    //private static Texture[] BuildButtons;
    //private static string[] options;
    public static void PopUpGUI(UnitScript forUnit)
    {
        if (forUnit.gameObject.GetInstanceID() != Focus.masterGameObject.GetInstanceID()) forUnit.gameObject.AddComponent<Focus>();
        Unit = forUnit;
        UnitPosition = MouseEvents.State.Position;
        //if (Unit.IsBuilding)
        //{
        //    options = Unit.Options.GetUnitsMenuOptions();
        //    BuildButtons = Unit.GetComponent<ProductionBuildingOptions>().GetButtons();
        //    MouseEvents.LEFTCLICK += MouseEvents_LEFTMouseEvents;   
        //}
        showGUI = true;
    }

    //static void MouseEvents_LEFTMouseEvents(Ray qamRay, bool hold)
    //{
    //    if (mainGUI.MainGuiArea.Contains((Vector2)MouseEvents.State.Position))
    //    {
    //        for (int i = 0; i < options.Length; i++)
    //        {
    //            BuildButtons[i]
    //        }
    //    }

    //}






    //private static string[] GetUnitsMenuOptions()
    //{
    //    return System.Enum.GetNames(Unit.Options.UnitState.GetType());
    //}



    void OnGUI()
    {
        if (showGUI)
        {
            float btnHeight = (40 * ScaleY);
            float zwischenbuttonraum=(20*ScaleY);
            string[] menuOptions = Unit.Options.GetUnitsMenuOptions();
          
            Rect guiposition;
            if (Unit.weapon.HasArsenal)
            {
                guiposition = new Rect(1718 * ScaleX, (210 * ScaleY) - 3 * guiStyle.fontSize, 202 * ScaleX, 360 * ScaleY);
                GUI.BeginGroup(guiposition,Unit.name+":\nSelect active Weapon" , guiSIDEstyle);
                for (int i = 0; i < Unit.weapon.arsenal; i++)
                {
                    if (GUI.Button(new Rect(0, 3 * guiStyle.fontSize + i * (btnHeight + zwischenbuttonraum), (180 * ScaleX), btnHeight), Unit.weapon.arsenal[i].ammunition.ToString()))
                    {
                        Unit.weapon.prefabSlot = Unit.weapon.arsenal[i];
                        showGUI = false;
                    }
                }
                if (GUI.Button(new Rect(0, 3 * guiStyle.fontSize + menuOptions.Length * (btnHeight + zwischenbuttonraum), (180 * ScaleX), btnHeight), "Cancel..."))
                {
                    showGUI = false;
                    GameObject.Destroy(Focus.masterGameObject.GetComponent<Focus>());
                }
                GUI.EndGroup();
            }
            if (Unit.IsBuilding)
            {
            //    guiposition = Camera.main.GetComponent<camScript>().mainGUI.MainGuiArea;
                guiposition = new Rect(1718 * ScaleX, (210 * ScaleY) - 3*guiStyle.fontSize, 202 * ScaleX, 360 * ScaleY);
          //      guiposition = new Rect(UnitPosition.x, UnitPosition.y, Pannel.texture.width * ScaleX, (menuOptions.Length + 1) * btnHeight + guiStyle.fontSize);
          //      Focus.masterGameObject.GetComponent<Focus>().SignOut();
                GUI.BeginGroup(guiposition, "Build Options for:\n" + Unit.name,guiSIDEstyle);
                for (int i = 0; i < menuOptions.Length; i++)
                {
                    if (GUI.Button(new Rect(0,3*guiStyle.fontSize +  i * (btnHeight + zwischenbuttonraum), (180*ScaleX), btnHeight), menuOptions[i]))
                 //   if (GUI.Button(new Rect(0, guiStyle.fontSize + i * btnHeight, Pannel.texture.width * ScaleX, btnHeight), menuOptions[i], buttonStyle))
                    {
                        Unit.Options.GiveOrder(i);
                        showGUI = false;
                    }
                }
                if (GUI.Button(new Rect(0, 3 * guiStyle.fontSize + menuOptions.Length * (btnHeight + zwischenbuttonraum), (180 * ScaleX), btnHeight), "Cancel..."))
             //   if (GUI.Button(new Rect(0, guiStyle.fontSize + menuOptions.Length * btnHeight, Pannel.texture.width * ScaleX, btnHeight), "Cancel...", buttonStyle))
                {
                    showGUI = false;
                }
                GUI.EndGroup();
            }
            else
            {
           //     Focus.masterGameObject.GetComponent<Focus>().SignOut();
                
                guiposition = new Rect(UnitPosition.x,view.height - UnitPosition.y, Pannel.texture.width * ScaleX, (menuOptions.Length + 1) * btnHeight + guiStyle.fontSize);
                GUI.BeginGroup(guiposition, "Orders for:\n "+Unit.name, guiStyle);

                for (int i = 0; i < menuOptions.Length; i++)
                {
                    if (GUI.Button(new Rect(0, guiStyle.fontSize + i * btnHeight, Pannel.texture.width * ScaleX, btnHeight), menuOptions[i], buttonStyle))
                    {
                        Unit.Options.GiveOrder(i);
                        showGUI = false;
                    }
                }
                if (GUI.Button(new Rect(0, guiStyle.fontSize + menuOptions.Length * btnHeight, Pannel.texture.width * ScaleX, btnHeight), "Cancel...", buttonStyle))
                {
                    showGUI = false;
                }
                GUI.EndGroup();
            }
        }
    }
	
	


	public void DoUpdate() 
    {

	}
}
