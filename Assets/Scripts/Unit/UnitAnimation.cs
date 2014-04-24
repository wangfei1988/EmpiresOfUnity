using UnityEngine;
using System.Collections;

abstract public class UnitAnimation : MonoBehaviour {

    public bool IsActive = true;
    public UnitAnimation anotherUnitAnimation;

    abstract internal void Animate();

    internal void DoUpdate()
    {
        if (IsActive) Animate();
        if(anotherUnitAnimation) anotherUnitAnimation.DoUpdate();
    }
	

}
