using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float newX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float newY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 newPos = transform.position;

        newPos.x += newX;
        newPos.y += newY;

        transform.position = newPos;
    }
}
