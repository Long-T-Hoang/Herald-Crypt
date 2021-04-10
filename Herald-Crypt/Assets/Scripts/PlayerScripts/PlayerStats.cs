using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private GameObject levelManager;

    public GameObject scoreUI;
    public GameObject healthUI;
    public GameObject moneyUI;

    private Text scoreTxt;
    private Text healthTxt;
    private Text moneyTxt;

    int score;
    int health;
    int money;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        health = 10;
        money = 0;

        // Already is a Text component
        //scoreTxt = GetComponent<Text>();
        healthTxt = healthUI.GetComponent<Text>();
        //moneyTxt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //scoreTxt.text = "Score: " + score;
        healthTxt.text = "Health: " + health;
        //moneyTxt.text = "Money: " + money;
    }

    public void Damaged(int dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            levelManager.GetComponent<LevelManager>().loadEndScreen();
        }
    }
}
