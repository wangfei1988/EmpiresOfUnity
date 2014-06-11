using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{

    public enum Resource
    {
        NANITEN,
        MATTER,
        MAXENERGY,
        ENERGY,
        LABORER, // Einheiten-Count
        MAXLABORER
    }

    public static Dictionary<Resource, uint> resourceList = new Dictionary<Resource, uint>();

    //static ResourceManager()
    void Start()
    {
        resourceList.Add(Resource.NANITEN, 50);
        resourceList.Add(Resource.MATTER, 50);
        resourceList.Add(Resource.MAXENERGY, 50);
        resourceList.Add(Resource.ENERGY, 50);
        resourceList.Add(Resource.LABORER, 0);
        resourceList.Add(Resource.MAXLABORER, 0);
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
        if (resourceList != null)
        {
            resourceList[resourceType] += addValue;
            return true;
        }
        return false;
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
