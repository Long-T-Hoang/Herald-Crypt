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
        attackTimer += Time.deltaTime;

        switch (currentState)
        {
            case EnemyState.IDLE:
                pfScript.CheckSurrounding();
                break;

            case EnemyState.FOLLOW:
                if(pfScript.DistanceToPlayer() < attackRange)
                {
                    currentState = EnemyState.ATTACK;
                }
                pfScript.FollowState();
                break;

            case EnemyState.ATTACK:
                player = pfScript.CheckSurrounding(); 

                if(player == null)
                {
                    anim.ResetAnimationFrame();
                    break;
                }

                Vector2 direction = player.transform.position - transform.position;
                float distance = direction.magnitude;

                hit = Physics2D.Raycast(transform.position, direction, distance, lineOfSightRay);

                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    Attack();
                }
                else
                {
                    currentState = EnemyState.FOLLOW;
                }
                }
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
