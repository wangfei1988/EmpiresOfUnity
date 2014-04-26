using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatedCursor : UnitComponent
{
    /* Member */
    public float AnimationFps = 3; // x Frages per Second
    public List<CursorObj> CursorList = new List<CursorObj>();

    /* Cursor ENUM */
    public enum CURSOR
    {
        STANDARD,
        CLICK,
        OVER_CLICKABLE_OBJECT,
        /*LOAD, WAIT, RESIZE, RESIZE_LR, RESIZE_TD, ... */
    }

    /* Vars */
    private GUIScript mainGUI;
    private int listIndex = 0;
    private int frameIndex = 0;
    private float time = 0;

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
    public CURSOR CurrentCursor
    {
        get
        {
            return this.CurrentCursor;
        }
        set
        {
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
                this.CurrentCursor = value;
                this.frameIndex = 0;
            }
        }
    }

    /* Start & Update */
    void Start()
    {
        mainGUI = this.GetComponent<GUIScript>();
        this.CurrentCursor = CURSOR.STANDARD;
    }

    internal override void DoUpdate()
    {
        SetCursor();
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

        /* Set Cursor */
        Cursor.SetCursor(
            AnimateCursor(),
            this.CursorList[this.listIndex].ClickPoint,
            CursorMode.Auto
        );
    }

    /* Check if an Building / Unit or Nothing [Standard] is at Mouse */
    private CURSOR CheckWhatIsUnderCursor()
    {
        if (MapViewArea.Contains((Vector2)MouseEvents.State.Position))
        {
            RaycastHit rayHit;
            if (Physics.Raycast(MouseEvents.State.Position, out rayHit))
            {
                this.guiText.text = rayHit.collider.gameObject.name + " at: " + MouseEvents.State.Position.AsWorldPointOnMap.ToString() + "\nID: " + rayHit.collider.gameObject.GetInstanceID().ToString();
                return CURSOR.OVER_CLICKABLE_OBJECT;
            }
            else
            {
                this.guiText.text = MouseEvents.State.Position.AsWorldPointOnMap.ToString();
            }
        }
        else
        {
            this.guiText.text = "";
        }
        return CURSOR.STANDARD;
    }

    /* Do Animation for Cursor */
    public Texture2D AnimateCursor()
    {
        this.time += Time.deltaTime;
        if (this.time >= 1 / this.AnimationFps)
        {
            this.time = 0;
            this.frameIndex++;
            if (this.frameIndex >= this.MaxFrame)
                this.frameIndex = 0;
        }
        return this.CursorList[this.listIndex].TextureList[this.frameIndex];
    }

}