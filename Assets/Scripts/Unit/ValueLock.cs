using UnityEngine;
using System.Collections;


public class ValueLock : UnitAnimation
{
    private Transform target;

    public bool lockPosition;
    public Vector3 position;
    public bool PositionX, PositionY, PositionZ;
    public bool lockRotation;
    public Vector3 rotation;
    public float rotationW;
    public bool RotationX, RotationY, RotationZ, RotationW;
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
        get { return lockRotation = RotationX | RotationY | RotationZ | RotationW; }
        set { lockRotation = RotationX = RotationY = RotationZ = RotationW = value; }
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
        if (LockRotation) target.rotation = new Quaternion(RotationX ? rotation.x : target.rotation.x, RotationY ? rotation.y : target.rotation.y, RotationZ ? rotation.z : target.rotation.z, RotationW ? rotationW : target.rotation.w);
        if (LockScale) target.localScale = new Vector3(ScaleX ? scale.x : target.localScale.x, ScaleY ? scale.y : target.localScale.y, ScaleZ ? scale.z : target.localScale.z);
    }



}
