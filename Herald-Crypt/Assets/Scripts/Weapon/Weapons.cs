using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons
{
    protected GameObject projectile;

    // Stats
    protected float atkCooldown;
    protected int attackPower;
    protected float range;
    protected float speed;

    public float AtkCooldown
    {
        get { return atkCooldown; }
    }    

    // Constructor
    public Weapons()
    {
        atkCooldown = 0.5f;
        attackPower = 1;
        range = 2.0f;
        speed = 5.0f;
    }

    public void Attack(Vector3 start, Vector3 end, Quaternion rotation)
    {
        Vector3 direction = (end - start).normalized;

        GameObject proj = GameObject.Instantiate(projectile, start, rotation) as GameObject;

        proj.GetComponent<ProjectileScript>().SetStat(range, speed, attackPower);
    }
}
