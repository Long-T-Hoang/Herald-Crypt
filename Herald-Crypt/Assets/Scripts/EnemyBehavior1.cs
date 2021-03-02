using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior1 : MonoBehaviour
{
    [SerializeField]
    private float detectionRange;
    [SerializeField]
    private LayerMask playerMask;

    private Collider2D playerCollider;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = null;
    }

    // Update is called once per frame
    void Update()
    {
        playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerMask.value);
        
        if (playerCollider != null)
        {
            player = playerCollider.gameObject;
        }
    }

    void RotateToPlayer()
    {

    }

    void MoveTo()
    {

    }
}
