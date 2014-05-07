using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//------------------------------------------------Container class for holding orders whitch units can store in mind for later processing...
public struct OrderChunk
{
    public EnumProvider.ORDERSLIST order;
    private float[] floats;
    public int data;


    public OrderChunk(EnumProvider.ORDERSLIST order,Vector3 point,int priorityAndOrID)
    {
        this.order = order;
        floats = new float[3];
        floats[0] = point.x;
        floats[1] = point.y;
        floats[2] = point.z;
        data = priorityAndOrID;
    }
    public Vector3 Vector
    { get { return new Vector3(floats[0], floats[1], floats[2]); } }
}
