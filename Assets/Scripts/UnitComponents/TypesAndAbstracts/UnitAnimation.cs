///<summary> UnitAnimation
///
///abstract class UnitAnimation....
///by: Kalle Münster
/// 
/// UnitAnimations can be Chaind in a row.
/// When the first animation's Updatefunction is called,
/// it will automaticly call the next one's Updatefunction 
/// after animating... the Updatefunction's will call the 
/// UnitAnimation's "Animate()" function, in which the code
/// for animation must be placed.
/// 
/// you can modyfie the Chainorder at runtime by using the 
/// "HookOnUpdata()" function.
/// to put a UnitAnimation after another UnitAnimation call:
/// "HookOnUpdata(UnitAnimation priorUnitAnimation)"
/// to put a UnitAnimation to the beginneng of the Chain, call 
/// "HookOnUpdata(UnitScript Unit)"
/// 
/// To Disable a UnitAnimation
/// Don't use the component's "enabled" property! 
/// instead set "IsActive" to false, otherwhise the chain will be broken
/// and the following UnitAnimations wo'nt be Updated anymore also.
/// 
///</summary>

using UnityEngine;
using System.Collections;

abstract public class UnitAnimation : MonoBehaviour {
    
    [SerializeField]
    private bool isActive = true;
    public bool IsActive
    {
        get { return isActive; }
        set
        {
            if (value != isActive)
                if (value && GetComponent<Rigidbody>()) GetComponent<Rigidbody>().isKinematic=false;

            isActive = value;
        }
    }

    public UnitAnimation NextUnitAnimation;

    abstract internal void Animate();

    //void Start()
    //{
    //    UpdateManager.OnUpdate += DoUpdate;
    //}

    internal void DoUpdate()
    {
        if (IsActive)
            Animate();
        if(NextUnitAnimation)
            NextUnitAnimation.DoUpdate();
    }

    internal void HookOnUpdata(UnitAnimation priorUnitAnimation)
    {
        priorUnitAnimation.NextUnitAnimation = this;
    }
    internal void HookOnUpdata(UnitScript Unit)
    {
        Unit.unitAnimation = this;
    }

}
