using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapons
{
    // Constructor
    public Sword()
    {
        attackPower = 1;
        atkCooldown = 0.2f;
        range = 2.5f;
        speed = 4.0f;
    }
}
