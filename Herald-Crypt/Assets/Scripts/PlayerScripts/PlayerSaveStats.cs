using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSaveStats
{
    private static int health = 10;
    private static string[] inventory = { "Sword", "", "" };
    private static int[] invAmmo = { 5, 0, 0 };
    private static int level = 1;

    public static int Level
    {
        get { return level; }

        set
        {
            if(value > 0)
            {
                level = value;
            }
        }
    }

    public static int Health
    {
        get { return health; }

        set
        {
            if (value > 0) health = value;
        }
    }

    public static string[] Inventory
    {
        get { return inventory; }

        set { inventory = value; }
    }

    public static int[] InvAmmo
    {
        get { return invAmmo; }

        set { invAmmo = value; }
    }

    public static int CalculateRoomNum()
    {
        float random = Random.Range(0.85f, 1.15f);
        int roomNum = 0;
        int lowerLimit = 5;
        int rangeMod = 5;
        float steepMod = 2;

        roomNum = Mathf.FloorToInt((Mathf.Pow(level, 1 / steepMod) * rangeMod + lowerLimit) * random) ;

        return roomNum;
    }
}
