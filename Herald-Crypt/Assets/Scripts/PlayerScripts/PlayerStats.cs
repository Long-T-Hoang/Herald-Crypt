using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private GameObject levelManager;
    private PlayerAnimation anim;

    public GameObject scoreUI;
    public GameObject healthUI;
    public GameObject moneyUI;
    public GameObject ammoUI;

    private Text scoreTxt;
    private Text healthTxt;
    private Text moneyTxt;
    private Text ammoTxt;

    //int score;
    int health;
    //int money;
    int ammo;

    private PlayerAttack playerAtt;
    private Weapons currentWeap;

    // Start is called before the first frame update
    void Start()
    {
        //score = 0;
        health = 10;
        //money = 0;

        // Already is a Text component
        //scoreTxt = GetComponent<Text>();
        healthTxt = healthUI.GetComponent<Text>();
        //moneyTxt = GetComponent<Text>();
        ammoTxt = ammoUI.GetComponent<Text>();
        ammoTxt.text = "";

        anim = GetComponentInChildren<PlayerAnimation>();
        playerAtt = GetComponentInChildren<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        //scoreTxt.text = "Score: " + score;
        healthTxt.text = "Health: " + health;
        //moneyTxt.text = "Money: " + money;

        ShowAmmo();
    }

    public void Damaged(int dmg)
    {
        health -= dmg;

        StartCoroutine(anim.DamagedAnimation());

        if(health <= 0)
        {
            levelManager.GetComponent<LevelManager>().loadGameOverScreen();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyProj"))
        {
            ProjectileScript temp = collision.GetComponent<ProjectileScript>();
            Damaged(temp.AttackPower);
        }
    }

    private void ShowAmmo(){
        if(playerAtt.GetCurrentWeapon() == null){
            return;
        }

        currentWeap = playerAtt.GetCurrentWeapon();
        ammoTxt.text = "Ammo: " + currentWeap.Ammo();

        if(currentWeap.Ammo() <= 0)
        {
            ammoTxt.text = "";
        }
    }
}
