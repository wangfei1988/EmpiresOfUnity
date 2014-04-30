using UnityEngine;
using System.Collections;

abstract public class Rocket : WeaponObject 
{
    public abstract bool LaunchButton
    { get; set; }
    public void TargetUpdatePosition(Vector3 targetPosition)
    {
        Target = targetPosition;
    }
    public void Launch(Vector3 targetPosition, UnitScript.GOODorEVIL friendOrfoe)
    {
        if (!LaunchButton)
        {
            Target = targetPosition;
            LaunchButton = true;
            this.GoodOrEvil = friendOrfoe;
        }
    }

}
