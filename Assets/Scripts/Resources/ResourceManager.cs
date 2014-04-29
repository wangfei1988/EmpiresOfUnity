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

}
