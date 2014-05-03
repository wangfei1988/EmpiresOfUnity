using UnityEngine;
using System.Collections;

/*
public class MouseEvents
{

    public enum MOUSEWHEELSTATE : sbyte
    {
        NONE = 0,
        WHEEL_UP = 1,
        WHEEL_DOWN = -1
    }

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

    public class MousePosition
    {
        private Collider Ground;
        private Camera Qam;
        private Vector2 position = Vector2.zero;
        public RaycastHit[] CursorRayHit;
        public NavMeshHit NMhit;
        private Ray? qamRay;

        public Ray AsRay
        {
            get
            {
                if (qamRay == null) qamRay = Qam.ScreenPointToRay(position);
                return qamRay.Value;
            }
        }
        private Vector3? worldPointOnMap;
        public Vector3 AsWorldPointOnMap
        {
            get
            {
                if (worldPointOnMap == null)
                {
                    RaycastHit groundHit;
                    if (Ground.collider.Raycast(AsRay, out groundHit, Qam.farClipPlane))
                        worldPointOnMap = groundHit.point;
                }
                if (worldPointOnMap != null) return worldPointOnMap.Value;
                else return Vector3.zero;
            }
        }

        public MousePosition()
        {
            Ground = GameObject.FindWithTag("Ground").collider;
            Qam = Camera.main;
        }
        public void SetNewMousePosition(Vector3 mousePut)
        {
            worldPointOnMap = null;
            qamRay = null;
            position = mousePut;

            NavMesh.Raycast(AsRay.origin, AsRay.direction * Qam.farClipPlane + AsRay.origin, out NMhit, NavMesh.GetNavMeshLayerFromName("Default"));

            CursorRayHit = Physics.RaycastAll(AsRay.origin, AsRay.direction, Qam.farClipPlane, Physics.AllLayers);
            for (int i = 0; i < CursorRayHit.Length; i++)
            {
                if (CursorRayHit[i].collider.gameObject.GetInstanceID() == Ground.gameObject.GetInstanceID())
                    worldPointOnMap = CursorRayHit[i].point;
            }
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

    public class MouseState
    {
        public MousePosition Position;
        private MouseButtonState[] buttons = new MouseButtonState[3];
        public MouseWheelState WHEEL;

        public static implicit operator Vector2(MouseState This)
        {
            return This.Position;
        }
        public static implicit operator bool(MouseState This)
        {
            return (This.buttons[0] | This.buttons[1] | This.buttons[2]);
        }
        public bool Hold
        {
            get { return (buttons[0].Hold | buttons[1].Hold | buttons[2].Hold); }
        }
        public MouseButtonState LEFT
        { get { return buttons[0]; } set { buttons[0] = value; } }
        public MouseButtonState RIGHT
        { get { return buttons[1]; } set { buttons[1] = value; } }
        public MouseButtonState MIDDLE
        { get { return buttons[2]; } set { buttons[2] = value; } }
    }

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

    public static MouseState State = new MouseState();

    static private GUIScript gameObject;
    static private bool MapClick;
    static private bool[] ButtonDown = new bool[3];
    static private bool[] hold = new bool[3];
    static private bool[] trigger = new bool[3];
    static private bool[] release = new bool[3];

    static internal void Setup(GameObject parrent)
    {
        gameObject = parrent.GetComponent<GUIScript>();
        State.Position = new MousePosition();
    }



    static private void GetMouseState()
    {
        State.Position.SetNewMousePosition(Input.mousePosition);
        MapClick = gameObject.MapViewArea.Contains(State);
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

        State.LEFT = new MouseButtonState(ButtonDown[0], hold[0]);
        State.RIGHT = new MouseButtonState(ButtonDown[1], hold[1]);
        State.MIDDLE = new MouseButtonState(ButtonDown[2], hold[2]);
        State.WHEEL = new MouseWheelState((int)(Input.GetAxis("Mouse ScrollWheel") * 100f));
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
        if (trigger[0] && ButtonDown[0] && MapClick)
            LEFTCLICK(State.Position, hold[0]);
        else if (release[0] && LEFTRELEASE != null)
            LEFTRELEASE();

        if (trigger[2] && ButtonDown[2] && MapClick)
            MIDDLECLICK(State.Position, hold[2]);
        else if (release[2] && MIDDLERELEASE != null)
            MIDDLERELEASE();

        if (trigger[1] && ButtonDown[1] && MapClick)
            RIGHTCLICK(State.Position, hold[1]);
        else if (release[1] && RIGHTRELEASE != null)
            RIGHTRELEASE();

        if (MOUSEWHEEL != null && State.WHEEL != MOUSEWHEELSTATE.NONE)
            MOUSEWHEEL(State.WHEEL);

    }

    static public void DoUpdate()
    {
        GetMouseState();
    }
}
*/


