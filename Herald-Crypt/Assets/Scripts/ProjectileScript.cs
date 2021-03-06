using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifetime;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Update timer
        timer += Time.deltaTime;

        // Update position
        Vector3 pos = transform.position;
        pos.x += transform.up.x * speed * Time.deltaTime;
        pos.y += transform.up.y * speed * Time.deltaTime;
        transform.position = pos;

        if(timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
