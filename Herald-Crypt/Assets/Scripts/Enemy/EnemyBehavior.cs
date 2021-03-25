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
        player = pfScript.CheckSurrounding();

        if(Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            Debug.Log("Attack");
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
