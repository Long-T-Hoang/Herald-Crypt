using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{ 
    public GameObject levelManager;
    private PlayerAnimation anim;

    public GameObject healthUI;
    public GameObject durabilityUI;

    [SerializeField]
    private GameObject[] hearts;
    [SerializeField]
    private GameObject[] durabilityBars;

    public Sprite[] durabilitySprites;

    private Text durabilityTxt;

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

        durabilityTxt = durabilityUI.GetComponent<Text>();
        durabilityTxt.text = "";

        anim = GetComponentInChildren<PlayerAnimation>();
        playerAtt = GetComponentInChildren<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
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
        durabilityTxt.text = "Weapon Durability: ";

        if(currentWeap.Durability() <= 0)
        {
            durabilityTxt.text = "";
        }

        for(int i=0; i < durabilityBars.Length; i++){
            if(i < currentWeap.Durability()){
                durabilityBars[i].SetActive(true);

                if(i == 0){
                    if(currentWeap.Durability() > 1){
                        durabilityBars[i].GetComponent<Image>().sprite = durabilitySprites[1];
                    }
                    else
                    {
                        durabilityBars[i].GetComponent<Image>().sprite = durabilitySprites[0];
                    }
                }
                else if(i == currentWeap.Durability() - 1){
                    durabilityBars[i].GetComponent<Image>().sprite = durabilitySprites[3];
                }
                else
                {
                    durabilityBars[i].GetComponent<Image>().sprite = durabilitySprites[2];
                }
            }
            else{
                durabilityBars[i].SetActive(false);
            }
        }
    }
}
