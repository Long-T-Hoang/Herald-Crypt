using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyBehavior : MonoBehaviour
{
    protected EnemyPathfinding pfScript;

    protected GameObject player;

    [Header("Enemy stats")]
    [SerializeField]
    protected int attackPower;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected int healthPoint;

    // Attack timer and cooldown
    protected const float ATK_COOLDOWN = 1.0f;
    protected float attackTimer;

    // Ref to animation script
    EnemyAnimation anim;

    // Line of sight
    RaycastHit2D hit;
    [SerializeField]
    LayerMask lineOfSightRay;

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
    protected virtual void Start()
    {
        pfScript = GetComponent<EnemyPathfinding>();
        currentState = EnemyState.IDLE;

        attackTimer = 0.0f;

        CR_running = false;

        anim = GetComponentInChildren<EnemyAnimation>();
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
                break;

            default:
                break;
        }
    }

    protected virtual void Attack()
    {
        float distance = pfScript.DistanceToPlayer(out player);

        if (distance > attackRange)
        {
            attackTimer = 0.0f;
            currentState = EnemyState.FOLLOW;
        }

        if (player != null && distance <= attackRange && attackTimer >= ATK_COOLDOWN)
        {
            Debug.Log("Attack");
            attackTimer = 0.0f;
            player.GetComponent<PlayerStats>().Damaged(attackPower);
        }

    }
    
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Projectiles"))
        {
            healthPoint -= collision.gameObject.GetComponent<ProjectileScript>().AttackPower;

            StartCoroutine(anim.DamageAnimation());

            if (healthPoint <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
