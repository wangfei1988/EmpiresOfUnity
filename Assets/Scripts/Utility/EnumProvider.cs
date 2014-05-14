
public class EnumProvider
{
    public enum UNITCLASS : int
    {
        GROUND_UNIT = 0,
        CONSTRUCTION_UNIT = 100,
        AIR_UNIT = 200,
        BUILDING = 300,
        PRODUCTION_BUILDING = 400,
    }

    public enum ORDERSLIST : int
    {
        MoveTo = 0,
        Produce = 1,
        Attack = 2,
        Build = 3,

        Guard = 10,
        Repaire = 11,

        Patrol = 20,
        MoveUnitsTo = 25,

        Hide = 50,
        Seek = 55,
        GlideFlight = 60,
        FullThrottle = 65,

        Stay = 100,
        StopProduction = 101,
        LandOnGround = 102,

        Upgrade = 1000,
        Cancel = 10000
    }

    public enum LAYERNAMES : int
    {
        Default = 0,
        TransparentFX=1,
        Ignore_Raycast=2,

        Water=4,

        Rectangles=8,
        Weapons=9,
        Units=10,
    }

    public enum UNITTYPE : int
    {
        Tank,
        Worker,
        RocketMan,
        Airport = 1000,
        Fabrik,
    }

    public enum DIRECTION : byte
    {
        forward,
        left,
        right,
        backward,
        up,
        down
    }

}




