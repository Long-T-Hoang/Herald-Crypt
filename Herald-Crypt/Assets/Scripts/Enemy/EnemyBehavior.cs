using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class EnemyBehavior : MonoBehaviour
{
    protected EnemyPathfinding pfScript;
        
    protected GameObject player;

    [Header("Enemy stats")]
    [SerializeField]
    protected int attackPower;
    [SerializeField]
    protected float attackRange;

    // Attack timer and cooldown
    protected const float ATK_COOLDOWN = 1.0f;
    protected float attackTimer;

    // Ref to animation script
    EnemyAnimation anim;

    // Possible states of enemy
    public enum EnemyState
    {
        IDLE,
        FOLLOW,
        ATTACK,
        ROAM,
        RETREAT
    }

    // Current state
    public EnemyState currentState;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pfScript = GetComponent<EnemyPathfinding>();
        currentState = EnemyState.IDLE;

        attackTimer = ATK_COOLDOWN;

        anim = gameObject.GetComponentInChildren<EnemyAnimation>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        attackTimer += Time.deltaTime;

        switch (currentState)
        {
            case EnemyState.IDLE:
                pfScript.CheckSurrounding();
                break;

            case EnemyState.FOLLOW:
                if(pfScript.DistanceToPlayer() < attackRange)
                {
                    anim.ResetAnimationFrame();
                    currentState = EnemyState.ATTACK;
                    break;
                }

                pfScript.FollowState();

                break;

            case EnemyState.ATTACK:
                Attack();
                break;

            default:
                break;
        }
    }

    protected virtual void Attack()
    {
        float distance = pfScript.DistanceToPlayer(out player);

        if(player != null && distance <= attackRange && attackTimer >= ATK_COOLDOWN)
        {
            //Debug.Log("Attack");
            attackTimer = 0.0f;
            player.GetComponent<PlayerStats>().Damaged(attackPower);
        }

        if(distance > attackRange)
        {
            attackTimer = 0.0f;
            anim.ResetAnimationFrame();
            currentState = EnemyState.FOLLOW;
        }
    }
    
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Projectiles"))
        {
            Destroy(gameObject);
        }
    }
}
