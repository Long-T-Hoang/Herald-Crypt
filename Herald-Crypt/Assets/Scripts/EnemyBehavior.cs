﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Enemy attributes")]
    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float detectionRange;
    [SerializeField]
    [Range(1.0f, 5.0f)]
    private float speed;

    [Header("Misc")]
    [SerializeField]
    private LayerMask playerMask;
    [SerializeField]
    private bool debugOn;

    // Player attributes
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

            RotateToPlayer();
            MoveTo(player.transform.position);
        }
    }

    private void RotateToPlayer()
    {

    }

    private void MoveTo(Vector2 pos)
    {
        Vector2 newPos = transform.position;
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        newPos.x += direction.x * speed * Time.deltaTime;
        newPos.y += direction.y *speed * Time.deltaTime;

        transform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Projectiles"))
        {
            Destroy(gameObject);
        }
    }


    // Debug info
    private void OnDrawGizmos()
    {
        if (!debugOn) return;

        // Detection range gizmo
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, detectionRange);
    }

    /*
    IEnumerator FollowPlayer()
    {

    }
    */
}