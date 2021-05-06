using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSaveStats
{
    private static int health = 10;
    private static string[] inventory = { "Sword", "", "" };
    private static int[] invAmmo = { 5, 0, 0 };

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
}
