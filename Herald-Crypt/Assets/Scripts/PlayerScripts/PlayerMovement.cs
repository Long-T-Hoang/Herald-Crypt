using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float newX = Input.GetAxis("Horizontal") * speed;
        float newY = Input.GetAxis("Vertical") * speed;

        rb.velocity = new Vector2(newX, newY);

        /*
        Vector3 newPos = transform.position;

        newPos.x += newX;
        newPos.y += newY;

        transform.position = newPos;
        */
    }
}
