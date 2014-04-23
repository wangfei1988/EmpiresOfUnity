using UnityEngine;
using System.Collections;

public class Follower : AnimaQuion 
{
    public Transform targetTransform;
    public bool faceForward;
    public Vector3 offSet;
    void Start()
    {

    }

    internal override void Animate()
    {
        if (targetTransform)
        {
            gameObject.transform.position = targetTransform.position + offSet;
            if (faceForward) gameObject.transform.forward = targetTransform.forward;
        }
    }
}
