using UnityEngine;
using System.Collections;

public class Cam : MonoBehaviour {

    public enum CAMERAMODE : byte
    {
        ORTHOGRAFIC,
        PERSPECTIVE,
        FIRSTPERSON,
    }
    public GUIScript mainGUI;
    private Quaternion Perspective_Rotation;
    private Quaternion Orthografic_Rotation;
    public float ORTHOGRAFIC_Y_HEIGHT = 50f;
    public float PERSPECTIVE_VIEW_SIZE = 20f;
    public float Orthografic_View_Size = 500f;
    private float Perspective_Y_Height = 50f;
    public CAMERAMODE qamMode = CAMERAMODE.ORTHOGRAFIC;
    private CAMERAMODE QamMode
    {
        get { return qamMode; }
        set
        {
            if(qamMode!=value)
                switch (value)
                {
                    case CAMERAMODE.ORTHOGRAFIC:
                        {
                            if (!gameObject.camera.isOrthoGraphic) { Perspective_Y_Height = gameObject.transform.position.y; Perspective_Rotation = gameObject.transform.rotation; }
                            gameObject.camera.enabled = false;
                            gameObject.transform.position = new Vector3(gameObject.transform.position.x, ORTHOGRAFIC_Y_HEIGHT, gameObject.transform.position.z+400);
                            gameObject.transform.rotation = Orthografic_Rotation;
                            gameObject.camera.orthographic = true;
                            gameObject.camera.orthographicSize = Orthografic_View_Size;
                            gameObject.camera.enabled = true;
                            break;
                        }
                    case CAMERAMODE.PERSPECTIVE:
                        {
                            if (gameObject.camera.isOrthoGraphic) { Orthografic_View_Size = gameObject.camera.orthographicSize; Orthografic_Rotation = gameObject.transform.rotation; }
                            gameObject.camera.enabled = false;
                            gameObject.camera.orthographic = false;
                            gameObject.camera.fieldOfView = PERSPECTIVE_VIEW_SIZE;
                            gameObject.transform.position = new Vector3(gameObject.transform.position.x, Perspective_Y_Height, gameObject.transform.position.z-400);
                            gameObject.transform.rotation = Perspective_Rotation;
                            gameObject.camera.enabled = true;
                            break;
                        }
                    case CAMERAMODE.FIRSTPERSON:
                        {
                            break;
                        }
                }
            qamMode = value;
        }
    }
  //  private float angelFactor;

	void Start () 
    {
        MouseEvents.MOUSEWHEEL += MouseEvents_MQUSEWHEEL;
  //      angelFactor = 1300f / 90f;
        Perspective_Rotation = new Quaternion(0.258819f, 0f, 0f, 0.9659258f);
        Orthografic_Rotation = new Quaternion(0.7071068f, 0f, 0f, 0.7071068f);
      
	}
     
    public 

    void MouseEvents_MQUSEWHEEL(MouseEvents.MOUSEWHEELSTATE wheelstate)
    {
        if (QamMode==CAMERAMODE.ORTHOGRAFIC)
        {
            gameObject.camera.orthographicSize -= (int)wheelstate * (camera.orthographicSize / 120f);
        }
        else if (QamMode == CAMERAMODE.PERSPECTIVE)
        {
            Vector3 buffer = gameObject.transform.position;
            buffer.y -= (int)wheelstate * (gameObject.transform.position.y / 120f);
            if (buffer.y < 1250f)
            {
                Ray centerRay = camera.ScreenPointToRay(new Vector3(camera.pixelWidth / 2f, camera.pixelHeight / 2f, 0f));
                RaycastHit groundHit;
                GameObject.FindGameObjectWithTag("Ground").collider.Raycast(centerRay, out groundHit, camera.farClipPlane);
                gameObject.transform.position = buffer;
                Vector3 newDirection = (groundHit.point - gameObject.transform.position).normalized;
                gameObject.transform.forward = newDirection;
            }
        }
    }

    public CAMERAMODE SwitchQam()
    {
        if (QamMode == CAMERAMODE.ORTHOGRAFIC) QamMode = CAMERAMODE.PERSPECTIVE;
        else if (QamMode == CAMERAMODE.PERSPECTIVE) QamMode = CAMERAMODE.ORTHOGRAFIC;

        return QamMode;
    }

	void Update () 
    {
	    
	}
}
