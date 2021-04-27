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

    private List<GameObject> heartObjs;
    public Texture2D heartTex;

    private Text scoreTxt;
    private Text moneyTxt;
    private Text ammoTxt;

    int health;
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
        //moneyTxt = GetComponent<Text>();

        ammoTxt = ammoUI.GetComponent<Text>();
        ammoTxt.text = "";

        anim = GetComponentInChildren<PlayerAnimation>();
        playerAtt = GetComponentInChildren<PlayerAttack>();

        ShowHearts(health);
    }

    // Update is called once per frame
    void Update()
    {
        //scoreTxt.text = "Score: " + score;
        //moneyTxt.text = "Money: " + money;

        ShowDurability();
    }

    public void Damaged(int dmg)
    {
        health -= dmg;

        StartCoroutine(anim.DamagedAnimation());

        if(health <= 0)
        {
            levelManager.GetComponent<LevelManager>().loadGameOverScreen();
        }
        ClearHearts();
        ShowHearts(health);
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
        heartObjs = new List<GameObject>();

        for(int i=0; i < hp; i++){
            heartObjs.Add(new GameObject("Heart"));
        }

        float heartX = 20f;
        float heartY = 510f;
        float heartGap = 35f;

        for(int i=0; i < heartObjs.Count; i++){
            heartObjs[i].transform.SetParent(healthUI.transform);
            heartObjs[i].transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            heartObjs[i].transform.position = new Vector3(heartX, heartY, 1f);

            Image heartImg = heartObjs[i].AddComponent<Image>();
            Sprite heartSpr = Sprite.Create(heartTex, new Rect(0f, 0f, heartTex.width, heartTex.height), new Vector2(0f, 0f));
            heartImg.sprite = heartSpr;

            heartX += heartGap;
        }
    }

    private void ClearHearts(){
        for(int i=0; i < heartObjs.Count; i++){
            Destroy(heartObjs[i]);
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
