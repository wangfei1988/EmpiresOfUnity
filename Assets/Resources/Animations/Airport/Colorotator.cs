using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/Unit Animations/Color-Rotator")]
public class Colorotator : UnitAnimation
{
    public Vector4 colorOChange = new Vector4(0,0,0,0);
    public Color32 color = new Color32(128, 128, 128, 255);
    public Component target;
    public enum targetAs
    {
        NotDefined = 0,
        SpriteRenderer,
        LightSource,
    }
    public targetAs TargetIs;
    public bool Cycle, Pingpong;

	void Start () 
    {
        
	}

    
    private byte addValuesAndClamp(byte bVal, bool clampCycle, bool clampPingpong, float fVal)
    {
        float output = pingChangeToPong ? (float)bVal + fVal : (float)bVal - fVal;
        if(clampCycle)
        {
            if (output > 255) output -= 255;
            else if (output < 0) output = 256 - output; 

        }
        else if (Pingpong)
        {
            if (output < 0) { output = -output; pingChangeToPong = true; }
            if (output > 255) { output = 256 - (output - 255); pingChangeToPong = false; }
        }
        else
        {
            if (output < 0) output = 0;
            if (output > 255) output = 255;
        }

        return (byte)output;
    }
    private bool pingChangeToPong = true;


    internal override void Animate()
    {
        color.r = addValuesAndClamp(color.r, Cycle, Pingpong, colorOChange.x);
        color.g = addValuesAndClamp(color.g, Cycle, Pingpong, colorOChange.y);
        color.b = addValuesAndClamp(color.b, Cycle, Pingpong, colorOChange.z);
        color.a = addValuesAndClamp(color.a, Cycle, Pingpong, colorOChange.w);

        switch (TargetIs)
        {
            case targetAs.SpriteRenderer:
                (target as SpriteRenderer).color = color;
                break;
            case targetAs.LightSource:
                (target as Light).color = color;
                break;
        }
        
    }
}
