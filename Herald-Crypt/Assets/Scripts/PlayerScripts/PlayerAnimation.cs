using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    [SerializeField]
    public Sprite[] animList;

    public int playerAnimPos;

    void Start() {
        
    }

    void Update() {
        //  Part - Rotate Sprite to Mouse Pos
        PivotPlayer();

        //  Part - Animate Player Movement
        if (playerAnimPos == 1) {
            GetComponent<SpriteRenderer>().sprite = animList[0];
        }

        else if (playerAnimPos == 2) {
            GetComponent<SpriteRenderer>().sprite = animList[1];
        }
    }

    //  MainMethod - Pivot Player
    //  Process : Rotates Player towards Mouse
    private void PivotPlayer() {
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.parent.position);
        Vector2 mouseDiff = new Vector2(Input.mousePosition.x - playerPos.x, Input.mousePosition.y - playerPos.y);

        float mouseDiffAng = Mathf.Atan2(mouseDiff.y, mouseDiff.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, mouseDiffAng));
    }
}
