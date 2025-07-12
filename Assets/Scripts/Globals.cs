using Unity.VisualScripting;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static bool hasGun = false;
    public static bool gunActive = true; // TODO: when grabbing a cube - deactivate the gun and canvas

    public static int health = 3;
    public static int ammo = 3;
    public static readonly int magazineCapacity = 10;

    public static bool yellowPressurePlateActive = false;
    public static bool redPressurePlateActive = false;
    public static bool bluePressurePlateActive = false;
    public static bool purplePressurePlateActive = false;


    public static void resetGlobalsToDefaults()
    {
        health = 3;
        ammo = 3;
        resetCubesForNextLevel();
    }

    public static void resetCubesForNextLevel()
    {
        yellowPressurePlateActive = false;
        redPressurePlateActive = false;
        bluePressurePlateActive = false;
        purplePressurePlateActive = false;
    }

    public static bool checkAllPressurePlatesActive()
    {
        return yellowPressurePlateActive && 
            redPressurePlateActive &&
            bluePressurePlateActive && 
            purplePressurePlateActive;
    }

}
