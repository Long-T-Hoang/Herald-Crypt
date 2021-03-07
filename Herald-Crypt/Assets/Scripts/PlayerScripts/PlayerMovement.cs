using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D rb;

    private float animCool = 0.3f;

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

        //  Part - Animating Player Sprite
        if (newX != 0 || newY != 0) {
            animCool -= Time.deltaTime;

            if (animCool <= 0) {
                if (transform.GetChild(1).gameObject.GetComponent<PlayerLook>().playerAnimPos == 1) {
                    transform.GetChild(1).gameObject.GetComponent<PlayerLook>().playerAnimPos = 2;
                }

                else if (transform.GetChild(1).gameObject.GetComponent<PlayerLook>().playerAnimPos == 2) {
                    transform.GetChild(1).gameObject.GetComponent<PlayerLook>().playerAnimPos = 1;
                }

                animCool = 0.3f;
            }
        }

        rb.velocity = new Vector2(newX, newY);

        /*
        Vector3 newPos = transform.position;

        newPos.x += newX;
        newPos.y += newY;

        transform.position = newPos;
        */
    }
}
