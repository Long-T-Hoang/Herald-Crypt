using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement
    [SerializeField]
    private float speed;
    private float MINIMUM_SPEED = 2.0f;
    private Vector2 movement;

    private Rigidbody2D rb;

    // Animation
    private float animCool = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float newX = Input.GetAxisRaw("Horizontal");
        float newY = Input.GetAxisRaw("Vertical");

        //  Part - Animating Player Sprite
        if (newX != 0 || newY != 0) {
            animCool -= Time.deltaTime;

            if (animCool <= 0) {
                if (transform.GetChild(1).gameObject.GetComponent<PlayerAnimation>().playerAnimPos == 1) {
                    transform.GetChild(1).gameObject.GetComponent<PlayerAnimation>().playerAnimPos = 2;
                }

                else if (transform.GetChild(1).gameObject.GetComponent<PlayerAnimation>().playerAnimPos == 2) {
                    transform.GetChild(1).gameObject.GetComponent<PlayerAnimation>().playerAnimPos = 1;
                }

                animCool = 0.3f;
            }
        }

        movement = new Vector2(newX, newY);
        //movement = SpeedClamp(movement);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    Vector2 SpeedClamp(Vector2 moveVector)
    {
        float magnitude = moveVector.magnitude;
        if (magnitude > 0.0f && magnitude < MINIMUM_SPEED) magnitude = MINIMUM_SPEED;

        moveVector = moveVector.normalized * magnitude;
        return moveVector;
    }
}
