using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainedOrders
{
    [SerializeField]
    private List<OrderChunk> orderList = new List<OrderChunk>();
    [SerializeField]
    private OrderChunk? current;

    public bool AllDone
    {
        get
        {
            return (!(orderList.Count>0))&(!current.HasValue);
        }
        set
        {
            if(value&(orderList.Count<1))
            current=null;
        }
    }
    public OrderChunk this[int index]
    {
        get { return orderList[index]; }
    }
    public void AddOrder(OrderChunk order)
    {
        orderList.Add(order);
    }
    public void AddOrder(EnumProvider.ORDERSLIST order,Vector3 point,int id)
    {
        orderList.Add(new OrderChunk(order,point,id));
    }
    public void AddOrderChain(ICollection<OrderChunk> orders)
    {
 	    orderList.AddRange(orders);
    }
    public void RemoveOrder(int index)
    {
        orderList.RemoveAt(index);
    }
    public void ForgetAllOrders()
    {
        orderList.Clear();
        current=null;
    }
    public OrderChunk Current()
    {
        if(current.HasValue)
        return current.Value;
        else return Next();
    }
    public OrderChunk Next()
    {
        if(orderList.Count>0)
        {
        current = orderList[0];
        orderList.RemoveAt(0);
        }
        return Current();
    }
}
