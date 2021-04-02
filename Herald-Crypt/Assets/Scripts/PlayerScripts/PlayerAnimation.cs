using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    public enum PlayerAnim {
        IDLE,
        MOVE,
        ATTACK
    }
    public PlayerAnim pAnim;

    [SerializeField]
    private float animCool;
    [SerializeField]
    private int animCount;

    public Sprite[] animMoveList;

    void Start() {
        
    }

    void Update() {
        //  Part - Rotate Sprite to Mouse Pos
        PivotPlayer();

        switch(pAnim) {
            case PlayerAnim.MOVE:
                MoveAnim();
                break;

            case PlayerAnim.ATTACK:
                animCount = 0;
                AttackAnim();
                break;
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

    // MainMethod - MoveAnim
    private void MoveAnim() {
        if (animCool < 0) {
            animCool = 0.25f;
            
            if (animCount < (animMoveList.Length - 1)) {
                animCount++;
            }

            else {
                animCount = 0;
            }
        }

        else {
            animCool -= Time.deltaTime;
        }

        GetComponent<SpriteRenderer>().sprite = animMoveList[animCount];
    }

    //  MainMethod - AttackAnim
    private void AttackAnim() {
        if (animCool < 0) {
            animCool = 0.25f;
            
            if (animCount < 1) {
                animCount++;
            }

            else {
                animCount = 0;
                pAnim = PlayerAnim.MOVE;
            }
        }

        else {
            animCool -= Time.deltaTime;
        }

        GetComponent<SpriteRenderer>().sprite = animMoveList[animCount];
    }
}
