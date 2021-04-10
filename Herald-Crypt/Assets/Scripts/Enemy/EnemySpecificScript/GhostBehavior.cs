using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GhostBehavior : EnemyBehavior
{
    [SerializeField]
    protected GameObject projectile;

    [SerializeField]
    protected float projectileSpeed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        attackTimer += Time.deltaTime;

        float distance = pfScript.DistanceToPlayer(out player);

        if (player != null && distance <= attackRange && attackTimer >= ATK_COOLDOWN)
        {
            //Debug.Log("Attack");
            attackTimer = 0.0f; 
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction); 

            GameObject proj = Instantiate(projectile, transform.position, rotation) as GameObject;

            proj.GetComponent<ProjectileScript>().SetStat(attackRange, projectileSpeed, attackPower);
        }

        if (distance > attackRange)
        {
            attackTimer = 0.0f;
            currentState = EnemyState.FOLLOW;
        }
    }
}
