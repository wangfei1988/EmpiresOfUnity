using UnityEngine;
using System.Collections;

public class Rotator : AnimaQuion 
{

    public float RotationModifierA;
	public float RotationModifierB;

    private float x, y, z;
    private float rotationmodifierX
    {
        get
        {
			if (RotationModifierA < 0f) RotationModifierA = -RotationModifierA;
            if (RotationModifierA > 100f) RotationModifierA = 100f;
            return RotationModifierA / 1000f; 
        }
        set 
		{ 
			
			RotationModifierA = 1000f * value; 
		}
    }
	private float rotationmodifierY
    {
        get
		{
			if (RotationModifierB < -50f) RotationModifierB = -50f;
            if (RotationModifierB > 100f) RotationModifierB = 100f;
			return RotationModifierB;
		}

        set { RotationModifierB = value; }
    }
    private bool up = true;

	void Start ()
	{
        x  = z = 0f;
        y = 0.5f;
	}


    internal override void Animate()
    {
        if (x < -rotationmodifierY * rotationmodifierX) up = true;
        else if (x > rotationmodifierY * rotationmodifierX) up = false;

        if (up) x += rotationmodifierX;
        else x -= rotationmodifierX;

        y = Mathf.Sin(x);

        z = y * (rotationmodifierX * rotationmodifierY / 3) + (-rotationmodifierX * rotationmodifierY / 4);
        this.gameObject.transform.Rotate(x, y, z);
    }
}
