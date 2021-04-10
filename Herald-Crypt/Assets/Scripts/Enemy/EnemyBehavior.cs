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
<<<<<<< HEAD
<<<<<<< HEAD
    protected float attackRange;

    // Attack timer and cooldown
    protected const float ATK_COOLDOWN = 1.0f;
    protected float attackTimer;

    // Ray cast for line of vision
    private RaycastHit2D hit;
    [SerializeField]
    private LayerMask lineOfSightRay;

    // Ref to animation script
    EnemyAnimation anim;
=======
    private float attackRange;
>>>>>>> parent of 6a74da8 (Added new enemy type)
=======
    private float attackRange;
>>>>>>> parent of 6a74da8 (Added new enemy type)

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
<<<<<<< HEAD
<<<<<<< HEAD
        attackTimer += Time.deltaTime;
        switch (currentState)
=======
        switch(currentState)
>>>>>>> parent of 6a74da8 (Added new enemy type)
=======
        switch(currentState)
>>>>>>> parent of 6a74da8 (Added new enemy type)
        {
            case EnemyState.IDLE:
                pfScript.CheckSurrounding();
                break;

            case EnemyState.FOLLOW:
<<<<<<< HEAD
<<<<<<< HEAD
                Debug.Log(currentState);
                if (pfScript.DistanceToPlayer() < attackRange)
=======
=======
>>>>>>> parent of 6a74da8 (Added new enemy type)
                pfScript.FollowState();

                if(pfScript.DistanceToPlayer() < attackRange)
>>>>>>> parent of 6a74da8 (Added new enemy type)
                {
                    currentState = EnemyState.ATTACK;
<<<<<<< HEAD
                }
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
=======
                }
                break;
>>>>>>> parent of 6a74da8 (Added new enemy type)

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
