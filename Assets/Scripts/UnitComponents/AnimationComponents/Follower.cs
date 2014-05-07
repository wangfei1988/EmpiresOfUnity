using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/Unit Animations/Follower")]
public class Follower : UnitAnimation 
{
    public Transform targetTransform;
    public bool faceForward;
    public Vector3 offSet;

    void Start()
    {

    }

    public void SetTransformToFollow(Transform followed)
    {
        targetTransform = followed;
    }
    public void SetTransformToFollow(GameObject followed)
    {
        targetTransform = followed.transform;
    }

    internal override void Animate()
    {
        if (targetTransform)
        {
            gameObject.transform.position = targetTransform.position + offSet;
            if (faceForward)
            {
                if (offSet == Vector3.zero)
                    gameObject.transform.forward = targetTransform.forward;
                else
                    gameObject.transform.forward = (targetTransform.position - this.gameObject.transform.position).normalized;
            }
        }
    }
}
