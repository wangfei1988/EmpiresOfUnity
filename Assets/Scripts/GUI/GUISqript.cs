using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;




[AddComponentMenu("Camera-Control/GUISqript")]
public class GUISqript : MonoBehaviour
{

    
    public static GameObject mainGUI;
    private static List<string> StaticTextLines = new List<string>();
    public static void AddTextLine(string line)
    {
        StaticTextLines.Insert(0,line);
    }

    public Vector2 scrollSpeed = new Vector2(1f, 1f);

    public UnitSqript.GOODorEVIL PlayerSide;
    public UnitGroup SellectedGroup;
    public int groupCount;
    private Quomponent animatedCursor;
    public RightClickMenu RightclickGUI;

    public GameObject SellectionSprite;
    public GUITexture SellectionGUITexture;
    private Rect sellectangle;

    public GameObject lastClickedUnit;

  //  public Qlick MouseEvents;


    new public Camera camera;
 //   private camScript QamAcsess;

    public Rect MapViewArea;
    public Rect MainGuiArea;

    public GUIText secondGUIText;
    private Vector2? mousePosition;
    public Vector2 MousePosition
    {
        get 
        {
            if (mousePosition == null) return (mousePosition = (Vector2)Qlick.State.Position).Value;
            else return mousePosition.Value;
        }
    }

    public Vector2 Scale;

    public List<GUIContent> mainGUIContent;

    private bool NoUnitFoqussed
    {
        get 
        {
            if (FoQus.masterGameObject) return !FoQus.masterGameObject.GetComponent<FoQus>();
            else return true;
        }
    }

    private bool UnitMenuIsOn
    {
        get { return RightClickMenu.showGUI; }
        set { RightClickMenu.showGUI = value; }
    }

    void Start()
    {
        mainGUI = this.gameObject;


      //  MouseEvents = ScriptableObject.CreateInstance<Qlick>();
        Qlick.Setup(gameObject);

        SellectedGroup = ScriptableObject.CreateInstance<UnitGroup>();
        SellectedGroup.startGroup();

        animatedCursor = (GetComponent<AnimatedMouseCursors>())?GetComponent<AnimatedMouseCursors>():null;
        
        //if (camera.name == null)
        //{
            foreach (Camera qam in UnityEngine.Object.FindObjectsOfType<Camera>())
                if (qam.name == "Main Camera")
                    camera = qam;
        //}
        Scale = new Vector2((camera.pixelRect.width / gameObject.guiTexture.texture.width), (camera.pixelRect.height / gameObject.guiTexture.texture.height));


   //     gameObject.guiTexture.pixelInset = new Rect(0, -camera.pixelHeight, camera.pixelWidth, camera.pixelHeight);
    //    if (gameObject.GetComponent<GUIText>() == null) gameObject.AddComponent<GUIText>();
        gameObject.guiText.pixelOffset = new Vector2(-Camera.main.pixelWidth / 2 + 25 * Scale.x, Camera.main.pixelHeight/2 - 20 * Scale.y);
        MapViewArea = new Rect(20 * Scale.x, 20 * Scale.y, 1675 * Scale.x, 1047 * Scale.y);
        MainGuiArea = new Rect(1716 * Scale.x, 20 * Scale.y, 184 * Scale.x, 1047 * Scale.y);
        gameObject.guiText.fontSize = (int)(40f * Scale.x + 0.5f);
        //guiTexture.pixelInset = MapViewArea;
        //guiTexture.guiTexture.border.left = (int)(20f * Scale.x);
        //guiTexture.guiTexture.border.right = (int)(225f * Scale.x);
        //guiTexture.guiTexture.border.top = (int)(20f * Scale.y);
        //guiTexture.guiTexture.border.bottom = (int)(13f * Scale.y);


    //    QamAcsess = Camera.main.GetComponent<camScript>();

        Qlick.LEFTQLICK += Qlick_LEFTQLICK;
        Qlick.RIGHTQLICK += Qlick_RIGHTQLICK;
        Qlick.LEFTRELEASE += Qlick_LEFTRELEASE;

        scrolling = false;
    }




    private GameObject ClickHitUnit(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            return hit.collider.gameObject;
        else return null;
    }


    void Qlick_LEFTQLICK(Ray qamRay, bool hold)
    {
        if (hold)
        {
            Sellectangle = new Rect(Sellectangle.x, Sellectangle.y, Sellectangle.xMin - MousePosition.x, Sellectangle.y - MousePosition.y);
        }
        else
        {
            Sellectangle = new Rect(MousePosition.x, MousePosition.y, 0, 0);
            if(NoUnitFoqussed) ClickHitUnit(qamRay).AddComponent<FoQus>();
        }
    }
    void Qlick_LEFTRELEASE()
    {
        SnapSellectangle();
    }

    void Qlick_RIGHTQLICK(Ray qamRay, bool hold)
    {
        if (NoUnitFoqussed)
        {
                GameObject clickedUnit = ClickHitUnit(qamRay);
                if (clickedUnit) clickedUnit.AddComponent<FoQus>();
        }
    }

    void OnGUI()
    {
        //GUI.BeginGroup(new Rect((1718 / Scale.x), (24 / Scale.y), (180 / Scale.x), (100 / Scale.y)));
        //if (GUI.Button(new Rect((0 / Scale.x), (0 / Scale.y), (180 / Scale.x), (40 / Scale.y)), guiContent[0])) { Application.Quit(); }
        //if (GUI.Button(new Rect((0 / Scale.x), (60 / Scale.y), (80 / Scale.x), (40 / Scale.y)), guiContent[2])) { }
        //if (GUI.Button(new Rect((100 / Scale.x), (60 / Scale.y), (80 / Scale.x), (40 / Scale.y)), guiContent[4])) { }

        GUI.BeginGroup(new Rect((1718*Scale.x ), (24*Scale.y ), (180 * Scale.x), (100 * Scale.y)));
        if (GUI.Button(new Rect((0 * Scale.x), (0 * Scale.y), (180 * Scale.x), (40 * Scale.y)), mainGUIContent[0])) { Application.Quit(); }
        if (GUI.Button(new Rect((0 * Scale.x), (60 * Scale.y), (80 * Scale.x), (40 * Scale.y)), mainGUIContent[1])) 
        {
            scrolling = !scrolling;

        }
        if (GUI.Button(new Rect((100 * Scale.x), (60 * Scale.y), (80 * Scale.x), (40 * Scale.y)), mainGUIContent[2])) 
        {
            Camera.main.GetComponent<QamSqript>().SwitchQam();
        }

        GUI.enabled = true;
        GUI.EndGroup();
    }







    private void SnapSellectangle()
    {
        SellectedGroup = SellectionSprite.GetComponent<SellectorSqript>().SnapSellection();
        Sellectangle = new Rect(Sellectangle.x, Sellectangle.y, 0f, 0f);
    }


    

    public Rect Sellectangle
    {
        get { return sellectangle; }
        set
        {
            sellectangle = value;
        //    RightclickGUI.Pannel.guiTexture.pixelInset = new Rect(value.x-Camera.main.pixelWidth / 2f,value.y- Camera.main.pixelHeight / 2f, -value.width, -value.height);
            SellectionGUITexture.pixelInset = new Rect(value.x - Camera.main.pixelWidth / 2f, value.y - Camera.main.pixelHeight / 2f, -value.width, -value.height);
        //    RightclickGUI.guiTexture.pixelInset = new Rect(value.x, value.y - guiTexture.pixelInset.height, value.width, value.height);
            Vector3 w1, w2;
            w1 = Camera.main.ScreenToWorldPoint(new Vector3(value.x, value.y, Camera.main.transform.position.y - 75));
            w2 = Camera.main.ScreenToWorldPoint(new Vector3(value.x + value.width, value.y + value.height, Camera.main.transform.position.y - 75));
            //   Sellection.transform.position = w1;
            SellectionSprite.transform.position = new Vector3(w1.x, 75, w1.z);
            SellectionSprite.transform.localScale = new Vector3(-(w2.x - w1.x), (w2.z - w1.z), 1f);
        }
    }
  //  public string MouseButtons;



    private bool scrolling=true;
    private void CheckForScrolling()
    {
        if (scrolling)
        {
            float x = 0;
            float y = 0;

            if (MousePosition.x < MapViewArea.xMin) x = scrollSpeed.x * -1;
            else if (MousePosition.x > MainGuiArea.xMax) x = scrollSpeed.x;

            if (MousePosition.y > MapViewArea.yMax) y = scrollSpeed.y;
            else if (MousePosition.y < MapViewArea.yMin) y = scrollSpeed.y * -1;

            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + x, Camera.main.transform.position.y, Camera.main.transform.position.z + y);
        }
    }


    private string TextField = "";
    private string TextUpdate()
    {
        TextField = guiText.text;
        int listlength = StaticTextLines.Count - 1;
        for (int i = listlength; i >= 0; i--)
        {
            TextField += StaticTextLines[i] + "\n";
            if (i > 6) StaticTextLines.RemoveAt(i);
        }
        return TextField;
    }

    void Update()
    {
        mousePosition = null;
        Qlick.DoUpdate();
        groupCount = SellectedGroup.Count;

        if (animatedCursor) animatedCursor.DoUpdate();
        CheckForScrolling();
        guiText.text = TextUpdate();
    }
}
