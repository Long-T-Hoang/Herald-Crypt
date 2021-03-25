using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;

    [Header("Weapon switch and inventory")]
    [SerializeField]
    private GameObject[] inventory;
    [SerializeField]
    private GameObject inventoryUI;
    [SerializeField]
    private GameObject defaultWeapon;
    private GameObject currentWeapon;
    private Weapons currentWepScript;
    public int currentWepIndex;
    private const int WEAPON_COUNT = 3;
    private UnityEvent invUpdateEvent;
    private UnityEvent switchWepEvent;
    private int currentWepNum;

    // Attack cooldown
    private float attackCooldownTimer;


    [SerializeField]
    private LayerMask weaponMask;

    public GameObject[] Inventory
    {
        get { return inventory; }
    }

    private void Awake()
    {
        // Set default weapon
        inventory[0] = Instantiate(defaultWeapon, new Vector3(transform.position.x, transform.position.y, -10), Quaternion.identity);
        currentWepNum = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentWepIndex = 0;

        attackCooldownTimer = 0.0f;

        invUpdateEvent = inventoryUI.GetComponent<InventoryUI>().InvUpdateEvent;
        switchWepEvent = inventoryUI.GetComponent<InventoryUI>().SwitchWepEvent;

        SwitchWeapon(0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Update timer
        if (attackCooldownTimer > 0.0f) attackCooldownTimer -= Time.deltaTime;

        // Switch weapon using middle mouse scroll
        float mouseScrollDelta = Input.mouseScrollDelta.y;
        if(mouseScrollDelta != 0.0f) SwitchWeapon(mouseScrollDelta);

        // Attack on mouse left click
        if(Input.GetMouseButton(0) && attackCooldownTimer <= 0.0f && inventory.Length > 0)
        {
            Attack();

            // Reset cooldown
            attackCooldownTimer = currentWepScript.AtkCooldown;
        }

        // Pickup item using E
        if(Input.GetKeyDown(KeyCode.E))
        {
            PickUpWeapon();
        }
    }

    void Attack()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

        currentWepScript.Attack(transform.position, mousePos, rotation);
    }

    void SwitchWeapon(float mouseScrollDelta)
    {
        currentWepIndex += (int)mouseScrollDelta;

        if (currentWepIndex < 0) currentWepIndex = currentWepNum - 1;
        if (currentWepIndex >= WEAPON_COUNT || currentWepIndex >= currentWepNum) currentWepIndex = 0;

        currentWeapon = inventory[currentWepIndex];
        currentWepScript = currentWeapon.GetComponent<Weapons>();

        switchWepEvent.Invoke();
    }

    void PickUpWeapon()
    {
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, 1.0f, weaponMask);

        if (weaponCollider == null) return;

        GameObject weaponToAdd = weaponCollider.gameObject;

        for(int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] == null)
            {
                currentWepNum++;
                AddToInventory(i, weaponToAdd);

                return;
            }
        }

        // Remove and put current weapon at player position then add weapon to inventory
        inventory[currentWepIndex].transform.position = transform.position;
        AddToInventory(currentWepIndex, weaponToAdd);
    }

    void AddToInventory(int index, GameObject weaponToAdd)
    {
        inventory[index] = weaponToAdd;
        weaponToAdd.transform.position = weaponToAdd.transform.position - Vector3.forward * 10;
        invUpdateEvent.Invoke();
    }
}
