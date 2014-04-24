using UnityEngine;
using System.Collections;


public class ValueLock : UnitAnimation
{
    public Transform target;

    public bool lockPosition;
    public Vector3 position;
    public bool[] Position = new bool[3];
    public bool lockRotation;
    public Vector3 rotation;
    public float rotationW;
    public bool[] Rotation = new bool[4];
    public bool lockScale;
    public Vector3 scale;
    public bool[] Scale = new bool[3];

    private bool LockPosition
    {
        get { return lockPosition = Position[0] | Position[1] | Position[2]; }
        set { Position[0] = Position[1] = Position[2] = lockPosition = value; }
    }
    private bool LockRotation
    {
        get { return lockRotation = Rotation[0] | Rotation[1] | Rotation[2] | Rotation[3]; }
        set { lockRotation = Rotation[0] = Rotation[1] = Rotation[2] = Rotation[3] = value; }
    }
    private bool LockScale
    {
        get { return lockScale = Scale[0] | Scale[1] | Scale[2]; }
        set { lockScale = Scale[0] = Scale[1] = Scale[2] = value; }
    }

	void Start ()    
    {
        if (!target) target = this.gameObject.transform;
	}

    internal override void Animate()
    {
        if (LockPosition) target.position = new Vector3(Position[0] ? position.x : target.position.x, Position[1] ? position.y : target.position.y, Position[2] ? position.z : target.position.z);
        if (LockRotation) target.rotation = new Quaternion(Rotation[0] ? rotation.x : target.rotation.x, Rotation[1] ? rotation.y : target.rotation.y, Rotation[2] ? rotation.z : target.rotation.z, Rotation[3] ? rotationW : target.rotation.w);
        if (LockScale) target.localScale = new Vector3(Scale[0] ? scale.x : target.localScale.x, Scale[1] ? scale.y : target.localScale.y, Scale[2] ? scale.z : target.localScale.z);
    }



}
