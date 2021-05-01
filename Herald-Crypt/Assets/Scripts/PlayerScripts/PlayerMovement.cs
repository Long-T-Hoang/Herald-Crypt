using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement
    [Header("Movement")]
    [SerializeField]
    private float speed;

    [Header("Dodge")]
    [SerializeField]
    private float dodgeCooldown;
    [SerializeField]
    private float dodgeDistance;
    [SerializeField]
    private float dodgeModifier;
    private bool hitWall;

    private float timer;
    private bool isDodge;
    private float MINIMUM_SPEED = 2.0f;
    private Vector2 movement;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        isDodge = false;
        timer = 0.0f;
        hitWall = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float newX = Input.GetAxisRaw("Horizontal");
        float newY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(newX, newY);
        //movement = SpeedClamp(movement);

        if(Input.GetKeyDown(KeyCode.LeftShift) && timer >= dodgeCooldown)
        {
            StartCoroutine(Dodge());
        }

        //  Part - Animating Player Sprite
        if (newX != 0 || newY != 0) {
            transform.GetChild(1).gameObject.GetComponent<PlayerAnimation>().pAnim = PlayerAnimation.PlayerAnim.MOVE;
        }
        else if (newX == 0 && newY == 0 && transform.GetChild(1).gameObject.GetComponent<PlayerAnimation>().pAnim == PlayerAnimation.PlayerAnim.MOVE) {
            transform.GetChild(1).gameObject.GetComponent<PlayerAnimation>().pAnim = PlayerAnimation.PlayerAnim.IDLE;
        }

    }

    private void FixedUpdate()
    {
        //rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        if(!isDodge) rb.velocity = movement * speed;
    }

    Vector2 SpeedClamp(Vector2 moveVector)
    {
        float magnitude = moveVector.magnitude;
        if (magnitude > 0.0f && magnitude < MINIMUM_SPEED) magnitude = MINIMUM_SPEED;

        moveVector = moveVector.normalized * magnitude;
        return moveVector;
    }

    IEnumerator Dodge()
    {
        Vector2 startPos = transform.position;
        float distance = 0;

        isDodge = true;
        timer = 0.0f;

        rb.AddForce(movement.normalized * dodgeModifier, ForceMode2D.Impulse);

        while(distance < dodgeDistance)
        {
            distance = Vector2.Distance(startPos, transform.position);
            yield return null;

            if (hitWall) break;
        }

        hitWall = false;
        isDodge = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall")) hitWall = true;
    }
}
