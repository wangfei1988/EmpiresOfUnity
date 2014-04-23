using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatedMouseCursors : Quomponent
{
    public enum CURSORS : int
    {
        STANDARD = 0,
        CLICK = 1,
        OVER_CLICKABLE_OBJECT = 2,
        WAIT = 3,
        

        RESIZE,
        RESIZE_LR,
        RESIZE_TD
    }
    private GUISqript mainGUI;
    public CURSORS CurrentCursor;
    public CURSORS cursor;
    public Vector2 CurrentClickPoint = new Vector2(1f, 1f);
    private int currentselectedList = 0;
	public List<Texture2D> cursorList;
    public List<Vector4> ranges;
    private int frameNumber=0;
    private int currentFrame
    {
        get { return (int)ranges[(int)CurrentCursor].x + frameNumber; }
    }
    private int MaxFrame
    {
        get { return (int)(ranges[(int)CurrentCursor].y - ranges[(int)CurrentCursor].x); }
    }


    void Start () 
    {
        mainGUI = GameObject.FindGameObjectWithTag("MainGUI").GetComponent<GUISqript>();
	}

    private Rect MapViewArea
    {
        get { return mainGUI.MapViewArea; }
    }

    public Texture2D GetNextFrame()
    {
        if(++frameNumber>MaxFrame)frameNumber=0;
        return CurrentCursorFrame;
    }

    
    private CURSORS Cursors
    {
        get { return CursorSellector; }
        set
        {
            if (Qlick.State.Hold) CursorSellector = cursor = CURSORS.CLICK;
            else if (value == CURSORS.CLICK) CursorSellector = cursor = CURSORS.STANDARD;
            else CursorSellector = cursor = value;
        }
    }
    private CURSORS CursorSellector
    {
        get { return CurrentCursor = (CURSORS)currentselectedList; }
        set
        {
            if (CurrentCursor != value)
            {
                CurrentCursor = value;
                currentselectedList = (int)CurrentCursor;
                frameNumber = 0;
                CurrentClickPoint = new Vector2(ranges[(int)CurrentCursor].z, ranges[(int)CurrentCursor].w);
            }
        }
    }

    public Texture2D CurrentCursorFrame
    {
        get { return cursorList[currentFrame]; }
    }
    private void CheckWhatsUnderCursor()
    {
        if (MapViewArea.Contains((Vector2)Qlick.State.Position))
        {
            RaycastHit what;
            if (Physics.Raycast(Qlick.State.Position, out what))
            {
                cursor = AnimatedMouseCursors.CURSORS.OVER_CLICKABLE_OBJECT;
                guiText.text = what.collider.gameObject.name + " at: " + Qlick.State.Position.AsWorldPointOnMap.ToString() + "\nID: " + what.collider.gameObject.GetInstanceID().ToString();
            }
            else
            {
                cursor = AnimatedMouseCursors.CURSORS.CLICK;
                guiText.text = Qlick.State.Position.AsWorldPointOnMap.ToString();
            }
        }
        else guiText.text = "";
    }
    private void AnimateCursor()
    {
        CheckWhatsUnderCursor(); 
        Cursors = cursor;
        Cursor.SetCursor(GetNextFrame(), CurrentClickPoint, CursorMode.Auto);
    }

    internal override void DoUpdate()
    {
        AnimateCursor();
    }


}