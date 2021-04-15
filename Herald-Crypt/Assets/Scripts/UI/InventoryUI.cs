using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventoryUI : MonoBehaviour
{
    private GameObject player;
    private PlayerAttack playerAtkScript;

    [SerializeField]
    private GameObject[] inventorySlots;
    private List<GameObject> inventory;

    private UnityEvent invUpdateEvent;
    private UnityEvent switchWeaponEvent;

    public UnityEvent InvUpdateEvent
    {
        get { return invUpdateEvent; }
    }

    public UnityEvent SwitchWepEvent
    {
        get { return switchWeaponEvent; }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAtkScript = player.GetComponent<PlayerAttack>();

        if (invUpdateEvent == null)
            invUpdateEvent = new UnityEvent();

        invUpdateEvent.AddListener(UpdateInventory);

        if (switchWeaponEvent == null)
            switchWeaponEvent = new UnityEvent();

        switchWeaponEvent.AddListener(HighlightSlot);

        UpdateInventory();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateInventory()
    {
        inventory = playerAtkScript.Inventory;

        // Loop through inventory and show sprite of current weapons in inventory
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventory.Count > i)
            {
                Image slotImage = inventorySlots[i].transform.GetChild(0).GetComponent<Image>();
                SpriteRenderer sr = inventory[i].GetComponent<SpriteRenderer>();

                slotImage.sprite = sr.sprite;
                slotImage.transform.localScale = new Vector2(2.0f, 2.0f);
                slotImage.SetNativeSize();
                slotImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                Image slotImage = inventorySlots[i].transform.GetChild(0).GetComponent<Image>();

                slotImage.sprite = null; 
                slotImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }
        }

        HighlightSlot();
    }

    private void HighlightSlot()
    {
        // Change color of current slot to red
        for(int i = 0; i < inventory.Count; i++)
        {
            Image currentSlot = inventorySlots[i].transform.GetChild(0).GetComponent<Image>();

            if (inventory[i] != null)
            {
                if (i == playerAtkScript.currentWepIndex)
                {
                    currentSlot.color = new Color(1.0f, 0.0f, 0.0f);
                }
                else
                {
                    currentSlot.color = new Color(1.0f, 1.0f, 1.0f);
                }
            }
        }
    }
}
