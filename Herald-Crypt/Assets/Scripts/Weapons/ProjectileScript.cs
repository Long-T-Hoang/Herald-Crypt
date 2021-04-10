using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]
    private string[] onDestroyTag;

    // Stats
    private float speed;
    private int attackPower;

    // Range before destroy
    public Vector3 spawnPos;
    private float range;

    // Components
    private Rigidbody2D rb;

    private Vector2 movement;

    // Timer
    //private float timer;
    
    public int AttackPower
    {
        get { return attackPower; }
    }

    // Constructor
    public void SetStat(float range, float speed, int attackPower)
    {
        this.attackPower = attackPower;
        this.speed = speed;
        this.range = range;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //timer = 0.0f;

        // Set move direction (forward in 2D)
        movement = transform.up;

        spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Update timer
        //timer += Time.deltaTime;
        float dist = Vector3.Distance(transform.position, spawnPos);

        if(dist >= range)
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
        foreach(string str in onDestroyTag)
        {
            if(collision.CompareTag(str))
            {
                Destroy(gameObject);
            }
        }
    }
}
