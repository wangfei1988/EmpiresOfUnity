using UnityEngine;
using System.Collections;

/*This class contains the buildmenu and the options
 *to build buildings. This will show in the GUI
 */

public class BuildMenu : MonoBehaviour 
{
    //List of all availbable buildings
    public enum Buildings
    {
        HeadQuarter,
        CivilHouse,
        TankFactory,
        Airport,
        Mine,
        Portal,
        Stock,
        Refinery,
        SolarTower
    }
}
