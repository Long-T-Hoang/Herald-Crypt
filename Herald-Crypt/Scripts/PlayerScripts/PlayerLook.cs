using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {
    [SerializeField]
    public Sprite playerAnim1;
    public Sprite playerAnim2;

    public int playerAnimPos;

    void Start() {
        
    }

    void Update() {
        //  Part - Rotate Sprite to Mouse Pos
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.parent.position);
        Vector2 mouseDiff = new Vector2(Input.mousePosition.x - playerPos.x, Input.mousePosition.y - playerPos.y);
        float mouseDiffAng = Mathf.Atan2(mouseDiff.y, mouseDiff.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, mouseDiffAng));

        //  Part - Animate Player Movement
        if (playerAnimPos == 1) {
            GetComponent<SpriteRenderer>().sprite = playerAnim1;
        }

        else if (playerAnimPos == 2) {
            GetComponent<SpriteRenderer>().sprite = playerAnim2;
        }
    }
}
