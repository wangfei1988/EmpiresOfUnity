/* UnitAnimation: Value Lock
 * by: Kalle Münster
 * It´s not a real Animation, because it does´nt cause any movement to the
 * object (in fact it prohibits movement - so its more like an 'anti'mation or so...) 
 * It Lock´s the sellected Values of the object´s transform to the given Values...
 * works best when put at the end of the UnitAnimations Chain.
 * All UnitAnimations put after this in the row will change
 * the Locked Values anyway, if they where locked or not.
 */
using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/Unit Animations/Value Lock (Lock´s Values in a Transform)")]
public class ValueLock : UnitAnimation
{
    private Transform target;

    public bool lockPosition;
    public Vector3 position;
    public bool PositionX, PositionY, PositionZ;
    public bool lockRotation;
    public Vector3 rotation;
    public bool RotationX, RotationY, RotationZ;
    public bool lockScale;
    public Vector3 scale;
    public bool ScaleX, ScaleY, ScaleZ;

    private bool LockPosition
    {
        get { return lockPosition = PositionX | PositionY | PositionZ; }
        set { PositionX = PositionY = PositionZ = lockPosition = value; }
    }
    private bool LockRotation
    {
        get { return lockRotation = RotationX | RotationY | RotationZ; }
        set { lockRotation = RotationX = RotationY = RotationZ = value; }
    }
    private bool LockScale
    {
        get { return lockScale = ScaleX | ScaleY | ScaleZ; }
        set { lockScale = ScaleX = ScaleY = ScaleZ = value; }
    }

	void Start ()    
    {
         target = this.gameObject.transform;
	}

    internal override void Animate()
    {
        if (LockPosition) target.position = new Vector3(PositionX ? position.x : target.position.x, PositionY ? position.y : target.position.y, PositionZ ? position.z : target.position.z);
        if (LockRotation) target.eulerAngles = new Vector3(RotationX ? rotation.x : target.eulerAngles.x, RotationY ? rotation.y : target.eulerAngles.y, RotationZ ? rotation.z : target.eulerAngles.z);
        if (LockScale) target.localScale = new Vector3(ScaleX ? scale.x : target.localScale.x, ScaleY ? scale.y : target.localScale.y, ScaleZ ? scale.z : target.localScale.z);
    }



}
