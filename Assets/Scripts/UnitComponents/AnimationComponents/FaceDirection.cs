/*UnitAnimation: Face Direction
 * 
 * The object will rotate to the sellected direction-source...
 */

using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/Unit Animations/Face Direction")]
public class FaceDirection : UnitAnimation
{

    public Transform TransformToFace;
    public bool faceMovingDirection, faceCamera,faceOtherTransform;
    public EnumProvider.DIRECTION forwardIs;
    public Vector3 direction;
    private UnitScript UNIT;
    private Vector3 qamDirection
    {
        get
        {
            if (Camera.main.GetComponent<Cam>().GetCamMode() == Cam.CAMERAMODE.PERSPECTIVE)
            {
                forwardIs = EnumProvider.DIRECTION.backward;
                return (Camera.main.transform.position - gameObject.transform.position).normalized;
            }
            else
            {
                forwardIs = EnumProvider.DIRECTION.forward;
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
            direction = (UNIT.Options as MovingUnitOptions).MovingDirection;
        else if (faceOtherTransform)
            direction = TransformToFace.position - this.gameObject.transform.position;
        else if (faceCamera)
            direction = qamDirection;

        if (direction != Vector3.zero)
        {
            switch (forwardIs)
            {
                case EnumProvider.DIRECTION.forward:
                    gameObject.transform.forward = direction;
                    break;
                case EnumProvider.DIRECTION.right:
                    gameObject.transform.right = direction;
                    break;
                case EnumProvider.DIRECTION.up:
                    gameObject.transform.up = direction;
                    break;
                case EnumProvider.DIRECTION.backward:
                    gameObject.transform.forward = -direction;
                    break;
                case EnumProvider.DIRECTION.left:
                    gameObject.transform.right = -direction;
                    break;
                case EnumProvider.DIRECTION.down:
                    gameObject.transform.up = -direction;
                    break;
            }
        }
    }
}
