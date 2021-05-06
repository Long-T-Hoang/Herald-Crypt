using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{ 
    public GameObject levelManager;
    private PlayerAnimation anim;

    public GameObject scoreUI;
    public GameObject healthUI;
    public GameObject moneyUI;
    public GameObject ammoUI;

    [SerializeField]
    private GameObject[] hearts;

    private Text scoreTxt;
    private Text moneyTxt;
    private Text ammoTxt;

    private int health;
    private int ammo;

    private PlayerAttack playerAtt;
    private Weapons currentWeap;

    public int Health
    {
        get { return health; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Load stat from PlayerSaveStats
        health = PlayerSaveStats.Health;

        ammoTxt = ammoUI.GetComponent<Text>();
        ammoTxt.text = "";

        anim = GetComponentInChildren<PlayerAnimation>();
        playerAtt = GetComponentInChildren<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        //scoreTxt.text = "Score: " + score;
        //moneyTxt.text = "Money: " + money;

        ShowDurability();
        ShowHearts(health);
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

    private void ShowHearts(int hp){
        for(int i=0; i < hearts.Length; i++){
            if(i < hp){
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }

    private void ShowDurability(){
        if(playerAtt.GetCurrentWeapon() == null){
            return;
        }

        currentWeap = playerAtt.GetCurrentWeapon();
        ammoTxt.text = "Weapon Durability: " + currentWeap.Durability();

        if(currentWeap.Durability() <= 0)
        {
            ammoTxt.text = "";
        }
    }
}
