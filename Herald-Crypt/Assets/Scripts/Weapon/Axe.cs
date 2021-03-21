using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapons
{
    // Constructor
    public Axe()
    {
        atkCooldown = 0.75f;
        attackPower = 2;
        range = 2.0f;
        speed = 5.0f;
    }
}
