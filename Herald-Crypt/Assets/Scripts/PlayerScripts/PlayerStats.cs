using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    public Text scoreTxt;
    public Text healthTxt;
    public Text moneyTxt;

    int score;
    int health;
    int money;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        health = 0;
        money = 0;

        // Already is a Text component
        //scoreTxt = GetComponent<Text>();
        //healthTxt = GetComponent<Text>();
        //moneyTxt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreTxt.text = "Score: " + score;
        healthTxt.text = "Health: " + health;
        moneyTxt.text = "Money: " + money;
    }
}
