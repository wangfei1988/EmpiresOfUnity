using UnityEngine;
using System.Collections;

abstract public class Rocket : WeaponObject 
{
    public abstract bool LaunchButton
    { get; set; }

    public void SetTarget(Vector3 targetPosition, FoE.GOODorEVIL foe)
    {
        Target = targetPosition;
        this.GoodOrEvil = new FoE(foe);
    }

    virtual public Vector3 Aim(Vector3 targetPosition)
    {
        Target = targetPosition;
        return ((Target - this.gameObject.transform.position).normalized / Vector3.Distance(Target, this.gameObject.transform.position)).normalized;
    }
}
