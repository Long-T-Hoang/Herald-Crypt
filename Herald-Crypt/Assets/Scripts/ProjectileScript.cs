using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifetime;

    // Components
    private Rigidbody2D rb;

    private Vector2 movement;

    // Timer
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timer = 0.0f;

        // Set move direction (forward in 2D)
        movement = transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        // Update timer
        timer += Time.deltaTime;

        if(timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        if(collision.transform.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
