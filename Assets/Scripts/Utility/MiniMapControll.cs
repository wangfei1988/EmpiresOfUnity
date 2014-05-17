using System.Net.Configuration;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections;

public class MiniMapControll : MonoBehaviour
{
    public static GameObject UnitBlip;
    public static GameObject BuildingBlip;
    public static GameObject EnemyUnitBlip;
    public static GameObject EnemyBuildingBlip;

    [SerializeField]
    GameObject[] blibloader;
    private Vector2 scale;
    public Texture GuiTextureBuffer;
    public Rect ClickableArea = new Rect(1800, 0, 120, 80);
    public bool ActiveButton = false;
    public bool IsActive
    {
        get { return this.camera.enabled; }
        set 
        {
            if (this.camera.enabled != value)
            {
                if (value)
                {
                    GuiTextureBuffer = GUIScript.main.guiTexture.texture;
                    GUIScript.main.guiTexture.texture = this.guiTexture.texture;
                    ClickableArea = new Rect(1470 * scale.x, 0 * scale.y, 450 * scale.x, 390 * scale.y);
                }
                else
                {
                    GUIScript.main.guiTexture.texture = GuiTextureBuffer;
                    ClickableArea = new Rect(scale.x * 1800, scale.y * 0, scale.x * 120, scale.y * 80);
                }
                this.camera.enabled = value;
            }
        }
    }

   void Awake()
   {
       UnitBlip = blibloader[0];
       BuildingBlip = blibloader[1];
       EnemyUnitBlip = blibloader[2];
       EnemyBuildingBlip = blibloader[3];
   }
   void Start()
   {
       scale = GUIScript.main.Scale;
       ClickableArea = new Rect(scale.x*1800,scale.y*0,scale.x*120,scale.y*80);
       GUIScript.MiniMap = this;
       UpdateManager.GUIUPDATE += UpdateManager_GUIUPDATE;
   }

    public void SwitchActive()
    {
        IsActive = !IsActive;
        this.ActiveButton = IsActive;
    }

   void UpdateManager_GUIUPDATE()
   {
       scale = GUIScript.main.Scale;
       if (this.ActiveButton == false)
       {
           if (ClickableArea.Contains((Vector2)MouseEvents.State.Position))
               IsActive = true;
           else
               IsActive = false;
       }

   }
}
