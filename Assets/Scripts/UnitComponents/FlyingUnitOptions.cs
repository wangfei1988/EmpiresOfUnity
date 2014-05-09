using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/UNIT/UnitOptions (Flying Units)")]
public class FlyingUnitOptions : GroundUnitOptions
{

    public override EnumProvider.UNITCLASS UNIT_CLASS
    {
        get { return EnumProvider.UNITCLASS.AIR_UNIT; }
    }
    public WingsAndJets Aviator;

    internal override void DoStart()
    {
        if (!this.gameObject.GetComponent<WingsAndJets>()) this.gameObject.AddComponent<WingsAndJets>();
        Aviator = this.gameObject.GetComponent<WingsAndJets>();
        base.DoStart();

    }

    

    internal override void DoUpdate()
    {

        
        base.DoUpdate();
        Aviator.DoUpdate();
    }

    
    

}
