using UnityEngine;
using System.Collections;

public class TouchCam : MonoBehaviour
{
    public float ZoomSpeed = 0.5f;

	void Start ()
	{
	    UpdateManager.OnUpdate += this.DoUpdate;
	}
	
	void DoUpdate () 
    {
	   this.TouchCamera();
	}

    void TouchCamera()
    {
        if (Input.touchCount == 2)
        {
            //Get two touches
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            //Get the position
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;


            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            camera.orthographicSize += deltaMagnitudeDiff * ZoomSpeed;

            camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);

        }
    }

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= this.DoUpdate;
    }
}
