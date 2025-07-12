using UnityEngine;

public class Globals : MonoBehaviour
{
    public static int health = 3;
    public static int ammo = 3;
    public static readonly int magazineCapacity = 10;

    public static void resetGlobalsToDefaults()
    {
        health = 3;
        ammo = 3;
    }

}
