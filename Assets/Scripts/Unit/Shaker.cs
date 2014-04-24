using UnityEngine;
using System.Collections;
using System;

public class Shaker : UnitAnimation
{
    public enum ROOTING : byte
    {
        mainPosition,
        mainRotation,
        mainScale,
        otherPosition,
        otherRotation,
        otherScale
    }

    public Transform mainTargetTransform;
    public Transform otherTargetTransform;
    public ROOTING sineRooting;
    public ROOTING triRooting;
    public Vector3 speed;
    public Vector3 amount;
    public bool crossModulate;
    public bool SinSetOrAdd;
    public bool TriSetOrAdd;
    
    private float a1 = 0;
    private float ValueX = 0;
    private float b1=0;
    private float ValueY =0;
    private float c1 = 0;
    private float ValueZ = 0;
    private Vector3 inverter = new Vector3(-1, -1, -1);
    public Vector3 Sin;
    public Vector3 Tri;

    void Start()
    {
        if (!mainTargetTransform) mainTargetTransform = this.gameObject.transform;
        if (!otherTargetTransform) otherTargetTransform = this.gameObject.transform;
    }

    private void ApplieByRooting(bool forSine, bool setOrAdd, Vector3 waveFrame, ROOTING rooting)
    {
        switch (rooting)
        {
            case ROOTING.mainPosition:
                {
                    mainTargetTransform.position = SetOrAdd(setOrAdd, mainTargetTransform.position, waveFrame);
                    break;
                }
            case ROOTING.mainRotation:
                {
                    mainTargetTransform.Rotate(SetOrAdd(setOrAdd, mainTargetTransform.eulerAngles, waveFrame));
                    break;
                }
            case ROOTING.mainScale:
                {
                    mainTargetTransform.localScale = SetOrAdd(setOrAdd, mainTargetTransform.localScale, waveFrame);
                    break;
                }
            case ROOTING.otherPosition:
                {
                    otherTargetTransform.position = SetOrAdd(setOrAdd, otherTargetTransform.position, waveFrame);
                    break;
                }
            case ROOTING.otherRotation:
                {
                    otherTargetTransform.Rotate(SetOrAdd(setOrAdd, otherTargetTransform.eulerAngles, waveFrame));
                    break;
                }
            case ROOTING.otherScale:
                {
                    otherTargetTransform.localScale = SetOrAdd(setOrAdd, otherTargetTransform.localScale, waveFrame);
                    break;
                }
        }
    }

    private Vector3 SetOrAdd(bool setOrAdd, Vector3 Target, Vector3 value)
    {
        if (setOrAdd) Target = value;
        else Target += value;
        return Target;
    }

    internal override void Animate()
    {
        a1 += speed.x * inverter.x;
        b1 += speed.y * inverter.y;
        c1 += speed.z * inverter.z;
        ValueX = Mathf.Sin(a1) * amount.x;
        ValueY = Mathf.Sin(b1) * amount.y;
        ValueZ = Mathf.Sin(c1) * amount.z;
        if (a1 >= 180) inverter.x = -1;
        else if (a1 <= -180) inverter.x = 1;
        if (b1 >= 180) inverter.y = -1;
        else if (b1 <= -180) inverter.y = 1;
        if (c1 >= 180) inverter.z = -1;
        else if (c1 <= -180) inverter.z = 1;
        Sin = new Vector3(ValueX, ValueY, ValueZ);
        ValueX = a1 / 180 * amount.x;
        ValueY = b1 / 180 * amount.y;
        ValueZ = c1 / 180 * amount.z;
        Tri = new Vector3(ValueX, ValueY, ValueZ);

        ApplieByRooting(true, SinSetOrAdd, Sin, sineRooting);
        ApplieByRooting(false, TriSetOrAdd, Tri, triRooting);

        if (crossModulate)
        {
            amount.x = Tri.z;
            amount.z = Tri.y;
            amount.y = Tri.x;
        }
    }
}
