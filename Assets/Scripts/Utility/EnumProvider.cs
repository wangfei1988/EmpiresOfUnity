using UnityEngine;
using System.Collections;
using System;

public struct FoE 
{
    public enum GOODorEVIL : int
    {
        Good = 1,
        Evil = -1,
        ENEMY = 0,
        FRIEND = 2,
    }

    private int FE;

    public FoE(FoE fe)
    {
        FE = fe.FE;
    }
    public FoE(GOODorEVIL fe_GE)
    {
        FE = (int)fe_GE;
    }
    public FoE(FoE a, FoE b)
    {
        FE = a.FE + b.FE;
        FE = (byte)this;
    }
    public static explicit operator byte(FoE This)
    {
        return (This.FE < 0) ? (byte)2 : (byte)This.FE;
    }
    public static implicit operator bool (FoE This)
    {
        return This.FE == 0; 
    }
    public static implicit operator GOODorEVIL(FoE This)
    {
        return (GOODorEVIL)This.FE;
    }
    public static FoE operator +(FoE a, FoE b)
    {
        return new FoE((GOODorEVIL)(a.FE + b.FE));
    }
    public string ToString()
    {
        return ((GOODorEVIL)FE).ToString();
    }
}

public class EnumProvider
{
    public enum ORDERSLIST : int
    {
        MoveTo = 0,
        Produce = 1,
        Attack = 2,
        Build = 3,

        Guard = 10,
        Repaire = 11,

        Patrol = 20,
        MoveUnitsTo = 25,

        Hide = 50,
        Seek = 55,

        Stay = 100,
        StopProduction = 101,

        Cancel = 10000
    }

    public enum LAYERNAMES : int
    {
        Default = 0,
        TransparentFX=1,
        Ignore_Raycast=2,

        Water=4,

        Rectangles=8,
        Weapons=9,
        Units=10,
    }

    public enum UNITTYPE : int
    {
        Tank,
        Worker,
        RocketMan,
        Airport = 1000,
        Fabrik,
    }
}




