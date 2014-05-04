using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Orderble
{
    public EnumProvider.ORDERSLIST? order;
    private float?[] floats;
    public int? data;


    public Orderble(EnumProvider.ORDERSLIST order,Vector3 point,int priorityAndOrID)
    {
        this.order = order;
        floats = new float?[3];
        floats[0] = point.x;
        floats[1] = point.y;
        floats[2] = point.z;
        data = priorityAndOrID;
    }
    public Vector3 Vector
    { get { return new Vector3(floats[0].Value, floats[1].Value, floats[2].Value); } }
}
