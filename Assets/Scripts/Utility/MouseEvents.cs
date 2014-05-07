using UnityEngine;
using System.Collections;


public class MouseEvents
{
    /// <summary>
    /// autor: Kalle Münster
    /// 
    /// A class whitch provides ClicEvents and gives static accses to the Mousedata... 
    /// 
    /// Actual MouseData can be accsessed via it's "State"-Property.
    /// the "State" property also holds other helpfull information like WorldPointOnMap, 
    /// UnitUnderCursor, Ray to cursorposition e.t.c... 
    /// to accsess them use "MouseEvents.State.Position" 
    /// Position holds screencoordinates when called as Vector2.
    /// when called or casted as Vector3, it holds WorldCoordinates as Point on the Map. 
    /// when casted to Ray, it returns a Ray from camera to Cursor...
    /// 
    /// </summary>

    #region//----------------Provided Click Events...
    public delegate void LeftClick(Ray qamRay, bool hold);
    public delegate void MiddleClick(Ray qamRay, bool hold);
    public delegate void RightClick(Ray qamRay, bool hold);
    public delegate void LeftRelease();
    public delegate void MiddleRelease();
    public delegate void RightRelease();
    public delegate void MouseWheelUPDOWN(MOUSEWHEELSTATE state);

    public static event LeftClick LEFTCLICK;
    public static event MiddleClick MIDDLECLICK;
    public static event RightClick RIGHTCLICK;
    public static event LeftRelease LEFTRELEASE;
    public static event MiddleRelease MIDDLERELEASE;
    public static event RightRelease RIGHTRELEASE;
    public static event MouseWheelUPDOWN MOUSEWHEEL;
    #endregion


    //-the "State"-object's class 
    public class MouseState
    {
        public enum GROUND : int
        {
            ZERO=0,
            ONE=1,
            TWO=2
        }
        public static GROUND CurrentGround;
        //--------Properties...
        public MousePosition Position;
        public MouseButtonState LEFT
        { get { return buttons[0]; } set { buttons[0] = value; } }
        public MouseButtonState RIGHT
        { get { return buttons[1]; } set { buttons[1] = value; } }
        public MouseButtonState MIDDLE
        { get { return buttons[2]; } set { buttons[2] = value; } }
        public bool Hold
        {
            get { return (buttons[0].Hold | buttons[1].Hold | buttons[2].Hold); }
        }
        public MouseWheelState WHEEL;

        // private variables...
        private MouseButtonState[] buttons = new MouseButtonState[3];

        //-castoperators.....
        public static implicit operator Vector2(MouseState This)
        {
            return This.Position;
        }
        public static implicit operator bool(MouseState This)
        {
            return (This.buttons[0] | This.buttons[1] | This.buttons[2]);
        }

        #region//MouseState-Nested's:
        //-class from which the "State->Position" instance is instanciated from...
        public class MousePosition
        {
            private UnitUnderCursor unitUnderCursor;
            public UnitScript AsUnitUnderCursor
            {
                get { return UnitUnderCursor.UNIT; }
            }

            private Collider[] Ground;
            private Camera Qam;
            private Vector2 position = Vector2.zero;

            private Ray? qamRay;

            public Ray AsRay
            {
                get
                {
                    if (qamRay == null) qamRay = Camera.main.ScreenPointToRay(position);
                    return qamRay.Value;
                }
            }
            private Vector3? worldPointOnMap;
            public Vector3 AsWorldPointOnMap
            {
                get
                {
                    RaycastHit groundHit;
                    if (worldPointOnMap == null)
                    {

                        if (Ground[(int)GUIScript.CurrentGround].collider.Raycast(AsRay, out groundHit, Camera.main.farClipPlane))
                            worldPointOnMap = groundHit.point;
                    }
                    if (worldPointOnMap != null) return worldPointOnMap.Value;
                    else return Vector3.zero;
                }
            }

            public MousePosition()
            {
                unitUnderCursor = new UnitUnderCursor();
                Ground = new Collider[3];
                Ground[0] = GameObject.FindWithTag("Ground").collider;
                Ground[1] = GameObject.FindWithTag("SubGround1").collider;
                Ground[2] = GameObject.FindWithTag("SubGround2").collider;
            }
            public void SetNewMousePosition(Vector3 mousePut)
            {
                worldPointOnMap = null;
                qamRay = null;
                position = mousePut;
                RaycastHit CursorRayHit;

                if (Physics.Raycast(AsRay.origin, AsRay.direction, out CursorRayHit))
                {
                    if (unitUnderCursor.Changed(CursorRayHit.collider.gameObject.GetInstanceID()))
                        unitUnderCursor.Set(CursorRayHit.collider.gameObject);
                }
                else
                    if (unitUnderCursor.Changed(-2))
                        unitUnderCursor.Set(null);
            }
            public static implicit operator Vector2(MousePosition This)
            {
                return This.position;
            }
            public static implicit operator Vector3(MousePosition This)
            {
                return This.AsWorldPointOnMap;
            }
            public static implicit operator Ray(MousePosition This)
            {
                return This.AsRay;
            }
        }

        //-class for the object holding the current Buttonstates...
        public class MouseButtonState
        {
            public readonly bool Pressed;
            public readonly bool Hold;
            public MouseButtonState(bool p, bool h)
            {
                Pressed = p;
                Hold = h;
            }
            public static implicit operator bool(MouseButtonState This)
            {
                return This.Pressed;
            }
        }

        //-class for holding actual Mousewheel state and data....
        public struct MouseWheelState
        {
            private MOUSEWHEELSTATE state;
            public MouseWheelState(int w)
            {
                state = (MOUSEWHEELSTATE)w;
            }
            public static implicit operator MOUSEWHEELSTATE(MouseWheelState This)
            {
                return This.state;
            }
            public static implicit operator int(MouseWheelState This)
            {
                return (int)This.state;
            }
        }
        #endregion

    }


    //the "State" property...    use it's "Position"-property for getting accses to Rays,Points,Cursors...
    public static MouseState State = new MouseState();


    //------private variables...
    static private GUIScript gameObject;
    static private bool MapClick;
    static private bool[] ButtonDown = new bool[3];
    static private bool[] hold = new bool[3];
    static private bool[] trigger = new bool[3];
    static private bool[] release = new bool[3];

    // private functions...
    static internal void Setup(GameObject parrent)
    {//-------------------------------------------------the startfunction for initialization at startup
        gameObject = parrent.GetComponent<GUIScript>();
        State.Position = new MouseState.MousePosition();
    }

    static private void GetMouseState()
    {//--------------------------------------------------------------The main funktion which retrives the Mouseinput... 
        State.Position.SetNewMousePosition(Input.mousePosition);
        MapClick = gameObject.MapViewArea.Contains(State);  //------checks if the click was on the Main-Map or if it was on other GUI elements...
        for (int i = 0; i < 3; i++)
        {
            trigger[i] = release[i] = false;
            if (Input.GetMouseButton(i))
            {

                if (ButtonDown[i])
                {
                    //hold
                    hold[i] = true;
                }
                else
                {
                    //click
                    ButtonDown[i] = true;
                }

            }
            else if (ButtonDown[i] && hold[i])
            {
                hold[i] = ButtonDown[i] = false;
                release[i] = true;
            }
            trigger[i] = checkEventForNull(i, ButtonDown[i]);

        }

        State.LEFT = new MouseState.MouseButtonState(ButtonDown[0], hold[0]);
        State.RIGHT = new MouseState.MouseButtonState(ButtonDown[1], hold[1]);
        State.MIDDLE = new MouseState.MouseButtonState(ButtonDown[2], hold[2]);
        State.WHEEL = new MouseState.MouseWheelState((int)(Input.GetAxis("Mouse ScrollWheel") * 100f));
        triggerEvents(trigger);
    }

    static private bool checkEventForNull(int number, bool pressed)
    {
        if (pressed)
            switch (number)
            {
                case 0: { return (LEFTCLICK != null); }
                case 1: { return (RIGHTCLICK != null); }
                case 2: { return (MIDDLECLICK != null); }
                default: return false;
            }
        else
        {
            switch (number)
            {
                case 0: { return (LEFTRELEASE != null); }
                case 1: { return (RIGHTRELEASE != null); }
                case 2: { return (MIDDLERELEASE != null); }
                default: return false;
            }
        }
    }

    static private void triggerEvents(bool[] trigger)
    {
        /* Left Click */
        if (trigger[0] && ButtonDown[0] && MapClick)
            LEFTCLICK(State.Position, hold[0]);
        else if (release[0] && LEFTRELEASE != null)
            LEFTRELEASE();

        /* Middle Click */
        if (trigger[2] && ButtonDown[2] && MapClick)
            MIDDLECLICK(State.Position, hold[2]);
        else if (release[2] && MIDDLERELEASE != null)
            MIDDLERELEASE();

        /* Right Click */
        if (trigger[1] && ButtonDown[1] && MapClick)
            RIGHTCLICK(State.Position, hold[1]);
        else if (release[1] && RIGHTRELEASE != null)
            RIGHTRELEASE();

        /* Mouse Wheel */
        if (MOUSEWHEEL != null && State.WHEEL != MOUSEWHEELSTATE.NONE)
            MOUSEWHEEL(State.WHEEL);

    }

    //-Updating...
    static public void DoUpdate()
    {
        
        GetMouseState();
    }

    public enum MOUSEWHEELSTATE : sbyte
    {
        NONE = 0,
        WHEEL_UP = 1,
        WHEEL_DOWN = -1
    }
}



