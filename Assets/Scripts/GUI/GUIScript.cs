using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/GUIScript")]
public class GUIScript : MonoBehaviour
{
    public static GUIScript main;
    private static List<string> StaticTextLines = new List<string>();
    public static MiniMapControll MiniMap;
    public static void AddTextLine(string line)
    {
        if (StaticTextLines != null)
            StaticTextLines.Insert(0,line);
    }

    public UnitGroup SelectedGroup;
    public int GroupCount;
    public RightClickMenu RightclickGUI;

    private SelectorScript SelectionSprite;
    private GroupRectangleScript GroupSprite;

    public GUITexture SellectionGUITexture;
    public GameObject lastClickedUnit;
    private string textField = "";
    public bool DebugText = false;

    private Scrolling scrolling;

    new public Camera camera;

    public Rect MapViewArea;
    public Rect MainGuiArea;

    public GUIText secondGUIText;
    private Vector2? mousePosition;
    public Vector2 MousePosition
    {
        get 
        {
            if (mousePosition == null)
                return (mousePosition = (Vector2)MouseEvents.State.Position).Value;
            return mousePosition.Value;
        }
    }

    public static Vector2 ScreenSize = new Vector2(Screen.width, Screen.height);

    public Vector2 Scale;

    public List<GUIContent> MainGuiContent;

    /* Lifebar Prefab */
    public Transform PrefabLifebar;

    /* Selection 3D Rectangle */
    private Rect selectionRectangle;
    public Rect SelectionRectangle
    {
        get { return selectionRectangle; }
        set
        {
            selectionRectangle = value;
            Vector3 w1;
            /* Get Mouse Position At Start (Left/Top Corner of Rect) */
            if (value.width == 0 && value.height == 0)
            {
                w1 = MouseEvents.State.Position.AsWorldPointOnMap;
                w1.y = SelectionSprite.transform.position.y;
                SelectionSprite.transform.position = w1;
            }
            else
            {
                w1 = SelectionSprite.transform.position;
            }
            /* Get Mouse Position At End (Width/Height of Rect) */
            Vector3 w2 = MouseEvents.State.Position.AsWorldPointOnMap;
            Vector3 localScale = new Vector3((w2.x - w1.x), -(w2.z - w1.z), 1);
            SelectionSprite.transform.localScale = localScale;
        }
    }

    private bool UnitFocused
    {
        get 
        {
            if (Focus.masterGameObject)
                return Focus.masterGameObject.GetComponent<Focus>();
            return false;
        }
    }

    private bool UnitMenuIsOn
    {
        get { return RightClickMenu.showGUI; }
        set { RightClickMenu.showGUI = value; }
    }

    void Awake()
    {
        main = this.gameObject.GetComponent<GUIScript>();
    }

    void Start()
    {
        foreach (GameObject rectangle in GameObject.FindGameObjectsWithTag("Rectangles"))
        {
            //if (rectangle.gameObject.name == "FocusRectangle")
                //FocusSprite = rectangle.GetComponent<FocusRectangleObject>();
            if (rectangle.gameObject.name == "SelectionRectangle")
                SelectionSprite = rectangle.GetComponent<SelectorScript>();
            else if (rectangle.gameObject.name == "GroupRectangle")
                GroupSprite = rectangle.GetComponent<GroupRectangleScript>();
        }
        
        MouseEvents.Setup(gameObject);

        SelectedGroup = ScriptableObject.CreateInstance<UnitGroup>();
        SelectedGroup.startGroup();

        scrolling = (GetComponent<Scrolling>()) ? GetComponent<Scrolling>() : null;
        
        //if (camera.name == null)
        //{
        camera = Camera.main;
        //}
        Scale = new Vector2((camera.pixelRect.width / gameObject.guiTexture.texture.width), (camera.pixelRect.height / gameObject.guiTexture.texture.height));


        //gameObject.guiTexture.pixelInset = new Rect(0, -camera.pixelHeight, camera.pixelWidth, camera.pixelHeight);
        //if (gameObject.GetComponent<GUIText>() == null) gameObject.AddComponent<GUIText>();

        gameObject.guiText.pixelOffset = new Vector2(-Camera.main.pixelWidth / 2 + 25 * Scale.x, Camera.main.pixelHeight/2 - 80 * Scale.y);
        MapViewArea = new Rect(20 * Scale.x, 20 * Scale.y, 1675 * Scale.x, 1047 * Scale.y);
        MainGuiArea = new Rect(1716 * Scale.x, 20 * Scale.y, 184 * Scale.x, 1047 * Scale.y);
        gameObject.guiText.fontSize = (int)(40f * Scale.x + 0.5f);

        //guiTexture.pixelInset = MapViewArea;
        //guiTexture.guiTexture.border.left = (int)(20f * Scale.x);
        //guiTexture.guiTexture.border.right = (int)(225f * Scale.x);
        //guiTexture.guiTexture.border.top = (int)(20f * Scale.y);
        //guiTexture.guiTexture.border.bottom = (int)(13f * Scale.y);


        //QamAcsess = Camera.main.GetComponent<camScript>();

        MouseEvents.LEFTCLICK += MouseEvents_LEFTCLICK;
        MouseEvents.RIGHTCLICK += MouseEvents_RIGHTCLICK;
        MouseEvents.LEFTRELEASE += MouseEvents_LEFTRELEASE;

        UpdateManager.GUIUPDATE += UpdateManager_GUIUPDATE;
    }

    void UpdateManager_GUIUPDATE()
    {
        mousePosition = null;
        GroupCount = SelectedGroup.Count;

        UpdateRectangles();
        if (DebugText)
            guiText.text = TextUpdate();


    }

    private void UpdateRectangles()

    {
        //FocusSprite.DoUpdate();
        GroupSprite.DoUpdate();
    }


    //private GameObject ClickHitUnit(Ray ray)
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit))
    //        return hit.collider.gameObject;
    //    else return null;
    //}

  //  public static UnitScript GetUnitUnderCursor()
  //  {
  /////      return main.animatedCursor.UnitUnderCursor.GetComponent<UnitScript>(); 
  //     /* RaycastHit hit;
  //      if (Physics.Raycast(ray, out hit))
  //          return hit.collider.gameObject;
  //      return null;*/
  //  }

    //public GameObject UnitUnderCursor
    //{
    //    get
    //    {
    //        return animatedCursor.UnitUnderCursor;
    //    }
    //}
    
    void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
    {
        if (hold)
        {
            SelectionRectangle = new Rect(SelectionRectangle.x, SelectionRectangle.y, MousePosition.x, MousePosition.y);
        }
        else
        {
            SelectionRectangle = new Rect(MousePosition.x, MousePosition.y, 0, 0);
            if (!hold)
                FocusUnit();
        }
    }

    void MouseEvents_LEFTRELEASE()
    {
        SnapSelectionRectangle();
    }

    void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold)
    {
        if (!hold)
            FocusUnit();
    }

    private void FocusUnit()
    {
        if (UnitFocused == false && UnitUnderCursor.UNIT)
        {
            UnitUnderCursor.gameObject.AddComponent<Focus>();
            UnitUnderCursor.UNIT.ShowLifebar();
        }
    }

    // Selection finished -> Now Select the Units inside the Area
    private void SnapSelectionRectangle()
    {
        // Get group of selected elements
        SelectedGroup = SelectionSprite.GetComponent<SelectorScript>().SnapSelection();

        // if group has 1 unit -> do single focus
        if (SelectedGroup.Count == 1)
        {
            GameObject unit = SelectedGroup[0];
            SelectedGroup.ResetGroup();
            unit.AddComponent<Focus>();
            unit.GetComponent<UnitScript>().ShowLifebar();
        }
        else
        {
            // Activate Lifebar at all selected units
            for (int i = 0; i < SelectedGroup.Count; i++)
                SelectedGroup[i].GetComponent<UnitScript>().ShowLifebar();
        }

        // Hide selection rectangle
        SelectionRectangle = new Rect(SelectionRectangle.x, SelectionRectangle.y, 0f, 0f);
    }

    void OnGUI()
    {
        //GUI.BeginGroup(new Rect((1718 / Scale.x), (24 / Scale.y), (180 / Scale.x), (100 / Scale.y)));
        //if (GUI.Button(new Rect((0 / Scale.x), (0 / Scale.y), (180 / Scale.x), (40 / Scale.y)), guiContent[0])) { Application.Quit(); }
        //if (GUI.Button(new Rect((0 / Scale.x), (60 / Scale.y), (80 / Scale.x), (40 / Scale.y)), guiContent[2])) { }
        //if (GUI.Button(new Rect((100 / Scale.x), (60 / Scale.y), (80 / Scale.x), (40 / Scale.y)), guiContent[4])) { }
        
        GUI.BeginGroup(new Rect((1718*Scale.x ), (24*Scale.y ), (180 * Scale.x), (160 * Scale.y)));
        if (GUI.Button(new Rect((0*Scale.x), (0*Scale.y), (180*Scale.x), (40*Scale.y)), MainGuiContent[0]) || Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("MainMenu");
        }
        
        if (GUI.Button(new Rect((0 * Scale.x), (60 * Scale.y), (80 * Scale.x), (40 * Scale.y)), MainGuiContent[1])) 
        {
            scrolling.SwitchScrollingStatus();
        }
        if (GUI.Button(new Rect((100 * Scale.x), (60 * Scale.y), (80 * Scale.x), (40 * Scale.y)), MainGuiContent[2])) 
        {
            //Camera.main.GetComponent<Cam>().SwitchCam();
            MiniMap.SwitchActive();
        }

        if (GUI.Button(new Rect((0 * Scale.x), (120 * Scale.y), (47 * Scale.x), (40 * Scale.y)), MainGuiContent[3]))
        {
            Ground.Switch(0);
        }
        if (GUI.Button(new Rect((68 * Scale.x), (120 * Scale.y), (47 * Scale.x), (40 * Scale.y)), MainGuiContent[4]))
        {
            Ground.Switch(1);
        }
        if (GUI.Button(new Rect((134 * Scale.x), (120 * Scale.y), (47 * Scale.x), (40 * Scale.y)), MainGuiContent[5]))
        {
            Ground.Switch(2);
        }
        GUI.enabled = true;
        GUI.EndGroup();
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Ground.Switch(0);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            Ground.Switch(1);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            Ground.Switch(2);
        }
        if (Input.GetKey(KeyCode.M))
        {
            MiniMap.SwitchActive();
        }
        /* Space Key Switch Camera */
        if (Input.GetKeyDown(KeyCode.Space))
            Camera.main.GetComponent<Cam>().SwitchCam();
    }

    private string TextUpdate()
    {
        textField = guiText.text;
        int listlength = StaticTextLines.Count - 1;
        for (int i = listlength; i >= 0; i--)
        {
            textField += StaticTextLines[i] + "\n";
            if (i < 6) StaticTextLines.RemoveAt(i);
        }
        return textField;
    }




}
