using UnityEngine;
using System.Collections;

public class FaceDirection : UnitAnimation
{

    public enum FACEDIRECTION : byte
    {
        forward,
        left,
        Right,
        backward,
        up,
        down
    }

    public Transform TransformToFace;
    public bool faceMovingDirection, faceQamera,faceOtherTransform;
    public FACEDIRECTION forwardIs;
    public Vector3 direction;
    private UnitScript UNIT;
    private Vector3 qamDirection
    {
        get
        {
            if (Camera.main.GetComponent<Cam>().qamMode == Cam.CAMERAMODE.PERSPECTIVE)
            {
                forwardIs = FACEDIRECTION.backward;
                return (Camera.main.transform.position - gameObject.transform.position).normalized;
            }
            else
            {
                forwardIs = FACEDIRECTION.forward;
                gameObject.transform.rotation = new Quaternion(0.707107f, 0f, 0f, 0.7071066f);
                return gameObject.transform.forward;
            }
        }
    }
    
	void Start ()
	{
	    TransformToFace = Camera.main.transform;
        if (this.gameObject.GetComponent<UnitOptions>()) UNIT = this.gameObject.GetComponent<UnitScript>();
	}

    internal override void Animate()
    {
        if (faceMovingDirection)
            direction = UNIT.Options.movingDirection;
        else if (faceOtherTransform)
            direction = TransformToFace.position - this.gameObject.transform.position;
        else if (faceQamera)
            direction = qamDirection;

        if (direction != Vector3.zero)
        {
            switch (forwardIs)
            {
                case FACEDIRECTION.forward:
                    gameObject.transform.forward = direction;
                    break;
                case FACEDIRECTION.Right:
                    gameObject.transform.right = direction;
                    break;
                case FACEDIRECTION.up:
                    gameObject.transform.up = direction;
                    break;
                case FACEDIRECTION.backward:
                    gameObject.transform.forward = -direction;
                    break;
                case FACEDIRECTION.left:
                    gameObject.transform.right = -direction;
                    break;
                case FACEDIRECTION.down:
                    gameObject.transform.up = -direction;
                    break;
            }
        }
    }
}
