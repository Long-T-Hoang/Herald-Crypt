using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapons
{
    // Constructor
    public Bow()
    {
        atkCooldown = 1.0f;
        attackPower = 1;
        range = 5.0f;
        speed = 7.0f;
    }
}
