using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyBehavior : MonoBehaviour
{
    EnemyPathfinding pfScript;

    GameObject player;

    [Header("Enemy stats")]
    [SerializeField]
    private int attackPower;
    [SerializeField]
    private float attackRange;

    private const float ATK_COOLDOWN = 1.0f;
    private float attackTimer;
    // Possible states of enemy
    public enum EnemyState
    {
        IDLE,
        FOLLOW,
        ATTACK,
        ROAM  
    }

    // Current state
    public EnemyState currentState;

    // Start is called before the first frame update
    void Start()
    {
        pfScript = GetComponent<EnemyPathfinding>();
        currentState = EnemyState.IDLE;

        attackTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case EnemyState.IDLE:
                pfScript.CheckSurrounding();
                break;

            case EnemyState.FOLLOW:
                pfScript.FollowState();

                if(pfScript.DistanceToPlayer() < attackRange)
                {
                    currentState = EnemyState.ATTACK;
                }
                break;

            case EnemyState.ATTACK:
                Attack();
                break;

            default:
                break;
        }
    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;

        float distance = pfScript.DistanceToPlayer(out player);

        if(player != null && distance <= attackRange && attackTimer >= ATK_COOLDOWN)
        {
            Debug.Log("Attack");
            attackTimer = 0.0f;
            player.GetComponent<PlayerStats>().Damaged(attackPower);
        }

        if(distance > attackRange)
        {
            attackTimer = 0.0f;
            currentState = EnemyState.FOLLOW;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Projectiles"))
        {
            Destroy(gameObject);
        }
    }
}
