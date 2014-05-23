using UnityEngine;
using System.Collections;

public abstract class UnitExtension : UnitComponent
{
    public override bool ComponentExtendsTheOptionalstateOrder
    {
        get
        {
            return true;
        }
    }
	// Use this for initialization
    protected override void SignIn()
    {
        UNIT.Options.Extensions_OnLEFTCLICK += OptionExtensions_OnLEFTCLICK;
        UNIT.Options.Extensions_OnRIGHTCLICK += OptionExtensions_OnRIGHTCLICK;
        base.SignIn();
    }
    protected override void SignOut()
    {
        UNIT.Options.Extensions_OnLEFTCLICK -= OptionExtensions_OnLEFTCLICK;
        UNIT.Options.Extensions_OnRIGHTCLICK -= OptionExtensions_OnRIGHTCLICK;
        base.SignOut();
    }
    
    internal abstract void OptionExtensions_OnLEFTCLICK(bool hold);
    internal abstract void OptionExtensions_OnRIGHTCLICK(bool hold);
}
