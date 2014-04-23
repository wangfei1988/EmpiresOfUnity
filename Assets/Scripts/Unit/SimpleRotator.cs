using UnityEngine;
using System.Collections;

public class SimpleRotator : AnimaQuion
{
    public Transform Target;
    public float X,Y,Z;

    void Start()
    {
        if (!Target) Target = this.gameObject.transform;
    }

    internal override void Animate()
    {
        Target.eulerAngles = new Vector3(Target.eulerAngles.x + X, Target.eulerAngles.y + Y, Target.eulerAngles.z + Z);
    }
}
