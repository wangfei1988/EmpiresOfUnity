using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{

    public enum Resource
    {
        WOOD,
        STONE,
        GOLD
    }

    public static Dictionary<Resource, uint> resourceList = new Dictionary<Resource, uint>();

    //static ResourceManager()
    void Start()
    {
        resourceList.Add(Resource.WOOD, 100);
        resourceList.Add(Resource.STONE, 100);
        resourceList.Add(Resource.GOLD, 100);
    }

    public static uint GetResourceCount(Resource resourceType)
    {
        uint value = 0;
        resourceList.TryGetValue(resourceType, out value);
        return value;
    }

    /*
     * USEAGE EXAMPLE
     * -  ResourceManager.SubtractResouce(Resource.GOLD, 5);
     * -  ResourceManager.AddResouce(Resource.STONE, 6);
     */
    public static bool AddResouce(Resource resourceType, uint addValue )
    {
        resourceList[resourceType] += addValue;
        return true;
    }

    public static bool SubtractResouce(Resource resourceType, uint subtractValue )
    {
        uint count = GetResourceCount(resourceType);
        if (count >= subtractValue)
        {
            resourceList[resourceType] -= subtractValue;
            return true;
        }

        return false;
    }

}
