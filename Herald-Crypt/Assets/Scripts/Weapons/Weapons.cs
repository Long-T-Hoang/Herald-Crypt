﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public GameObject projectile;
    public GameObject prefabRef;

    // Stats
    [SerializeField]
    protected float atkCooldown;
    [SerializeField]
    protected int attackPower;
    [SerializeField]
    protected float range;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int usedCount;

    public bool pickedUp;

    public float AtkCooldown
    {
        get { return atkCooldown; }
    }    

    public int UsedCount
    {
        get { return usedCount; }
        set
        {
            if (value > 0) usedCount = value;
        }
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

        GameObject proj = Instantiate(projectile, start, rotation) as GameObject;

        proj.GetComponent<ProjectileScript>().SetStat(range, speed, attackPower);

        usedCount--;

        if(usedCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int Durability(){
        if(usedCount <= 0)
        {
            return 0;
        }
        return usedCount;
    }
}
