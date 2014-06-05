using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Animated Cursor Component
 * Set a List of [CursorObj]
 * @date 2014-04-26
 */
public class AnimatedCursor : MonoBehaviour
{
    /* Member */
    public List<CursorObj> CursorList = new List<CursorObj>();
    public bool LockCursor = false;

    /* Cursor ENUM */
    public enum CURSOR
    {
        NONE,
        STANDARD,
        CLICK,
        OVER_CLICKABLE_OBJECT,
        OVER_DISABLED_OBJECT,
        DRAGnDROP,
        /*LOAD, WAIT, RESIZE, RESIZE_LR, RESIZE_TD, ... */
    }

    /* Vars */
    private GUIScript mainGUI;
    private int listIndex = -1;
    private int frameIndex = 0;
    private float time = 0;

    /* RayCast for UnderCursor */
  //  public static GameObject UnitUnderCursor;

    /* Properties */
    private Rect MapViewArea
    {
        get { return mainGUI.MapViewArea; }
    }

    /* Get Count of Texture2D's at current Cursor, used for Animations */
    private int MaxFrame
    {
        get
        {
            return this.CursorList[listIndex].TextureList.Count;
        }
    }

    /* Cursor Property, don't use cursor to save, but use CurrentCursor as a Setter */
    private CURSOR cursor = CURSOR.NONE;
    public CURSOR CurrentCursor
    {
        get
        {
            //return this.CurrentCursor;
            return this.cursor;
        }
        set
        {
            if (this.cursor == value)
                return;
            int newIndex = -1;
            for(int i = 0; i < CursorList.Count; i++)
            {
                if (CursorList[i].CursorType == value)
                {
                    newIndex = i;
                    break;
                }
            }
            if (newIndex != -1 && this.listIndex != newIndex)
            {
                this.listIndex = newIndex;
                this.cursor = value;
                this.frameIndex = 0;
            }
        }
    }

    /* Start & Update */
    void Start()
    {
        mainGUI = this.GetComponent<GUIScript>();
        this.CurrentCursor = CURSOR.STANDARD;
    //    UpdateManager.OnUpdate += DoUpdate;
        UpdateManager.OnMouseUpdate += DoUpdate;
    }

    void DoUpdate()
    {
        if(LockCursor == false)
            SetCursor();

        SetCursorToUnity();
    }

    /* Methods */
    private void SetCursor()
    {
        /* Check Cursor */
        if (MouseEvents.State.Hold)
        {
            this.CurrentCursor = CURSOR.CLICK;
        }
        else
        {
            CURSOR underMouse = CheckWhatIsUnderCursor();
            this.CurrentCursor = underMouse;
        }
    }
    private void SetCursorToUnity()
    {

        /* Set Cursor */
        Cursor.SetCursor(
            AnimateCursor(),
            this.CursorList[this.listIndex].ClickPoint,
            CursorMode.Auto
        );
    }

    /* Check if an Building / Unit or Nothing [Standard] is at Mouse*/
    private CURSOR CheckWhatIsUnderCursor()
    {
        if (MouseEvents.State.Position.IsOverMainMapArea||MouseEvents.State.Position.IsOverMiniMapArea)
        {
           
            if (MouseEvents.State.Position.AsUnitUnderCursor)
            {
                if (InGameText.ShowInfo)
                    this.guiText.text = UnitUnderCursor.gameObject.name + " at: " + MouseEvents.State.Position.AsWorldPointOnMap.ToString() + "  - ID: " + UnitUnderCursor.gameObject.GetInstanceID().ToString();
                return CURSOR.OVER_CLICKABLE_OBJECT;
            }
            else
            {
                if (InGameText.ShowInfo)
                    this.guiText.text = MouseEvents.State.Position.AsWorldPointOnMap.ToString();
            }
        }
        else
        {
            if (InGameText.ShowInfo)
                this.guiText.text = "";
        }
        return CURSOR.STANDARD;
    }



    /* Do Animation for Cursor */
    public Texture2D AnimateCursor()
    {
        this.time += Time.deltaTime;
        if (this.time >= 1 / this.CursorList[this.listIndex].AnimationFps)
        {
            this.time = 0;
            this.frameIndex++;
            if (this.frameIndex >= this.MaxFrame)
                this.frameIndex = 0;
        }
        return this.CursorList[this.listIndex].TextureList[this.frameIndex];
    }

}