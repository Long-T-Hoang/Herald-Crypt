using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    [SerializeField]
    private float animCool;
    [SerializeField]
    private int currentAnimFrame;
    [SerializeField]
    private GameObject player;

    public Sprite[] animMoveList;
    public Sprite[] animAttackList;

    void Start() {
        player = GameObject.Find("Player");
    }

    void Update() {
        switch(transform.parent.gameObject.GetComponent<EnemyBehavior>().currentState) {
            case EnemyBehavior.EnemyState.FOLLOW:
                PivotEnemy();
                MoveAnim();
                break;

            case EnemyBehavior.EnemyState.ATTACK:
                PivotEnemy();
                AttackAnim();
                break;
        }
    }

    //  MainMethod - Pivot Enemy
    //  Process : Rotates Enemy towards Player
    private void PivotEnemy() {
        Vector3 enemyPos = transform.parent.position;
        Vector3 playerPos = player.transform.position;
        Vector2 playerDiff = new Vector2(playerPos.x - enemyPos.x, playerPos.y - enemyPos.y);

        float playerDiffAng = Mathf.Atan2(playerDiff.y, playerDiff.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, playerDiffAng));
    }

    // MainMethod - MoveAnim
    private void MoveAnim() {
        if (animCool < 0) {
            animCool = 0.25f;
            
            if (currentAnimFrame < animMoveList.Length - 1) {
                currentAnimFrame++;
            }

            else {
                currentAnimFrame = 0;
            }
        }

        else {
            animCool -= Time.deltaTime;
        }

        GetComponent<SpriteRenderer>().sprite = animMoveList[currentAnimFrame];
    }

    // MainMethod - AttackAnim
    private void AttackAnim() {
        if (animCool < 0) {
            animCool = 0.1f;

            currentAnimFrame++;

            if (currentAnimFrame >= animAttackList.Length - 1)
            {
                currentAnimFrame = 0;
                transform.parent.GetComponent<EnemyBehavior>().currentState = EnemyBehavior.EnemyState.IDLE;
            }
        }

        else {
            animCool -= Time.deltaTime;
        }

        //GetComponent<SpriteRenderer>().sprite = animAttackList[animCount];
    }

    // Reset current animation frame to 0
    public void ResetAnimationFrame()
    {
        currentAnimFrame = 0;
    }
}
