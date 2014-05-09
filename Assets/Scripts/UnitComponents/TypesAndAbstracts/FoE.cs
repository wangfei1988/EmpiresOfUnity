using UnityEngine;
using System.Collections;

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
    public static implicit operator bool(FoE This)
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
    new public string ToString()
    {
        return ((GOODorEVIL)FE).ToString();
    }
}
