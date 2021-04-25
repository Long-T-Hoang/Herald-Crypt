using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;

    [Header("Weapon switch and inventory")]
    private List<GameObject> inventory;
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

    // Attack cooldown
    private float attackCooldownTimer;

    public bool paused;

    [SerializeField]
    private LayerMask weaponMask;

    public List<GameObject> Inventory
    {
        get { return inventory; }
    }

    private void Awake()
    {
        // Set default weapon
        inventory = new List<GameObject>();
        inventory.Add(Instantiate(defaultWeapon, new Vector3(transform.position.x, transform.position.y, -10), Quaternion.identity));
    }

    // Start is called before the first frame update
    void Start()
    {
        currentWepIndex = 0;

        attackCooldownTimer = 0.0f;

        invUpdateEvent = inventoryUI.GetComponent<InventoryUI>().InvUpdateEvent;
        switchWepEvent = inventoryUI.GetComponent<InventoryUI>().SwitchWepEvent;

        SwitchWeapon(0.0f);

        paused = false;
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
        if(Input.GetMouseButton(0) && attackCooldownTimer <= 0.0f && inventory.Count > 0)
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

        // Update inventory UI
        //invUpdateEvent.Invoke();
        UpdateInv();
    }

    void Attack()
    {
        if (paused) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

        if (currentWeapon != null)
        {
            currentWepScript.Attack(transform.position, mousePos, rotation);
        }

        transform.GetChild(1).gameObject.GetComponent<PlayerAnimation>().pAnim = PlayerAnimation.PlayerAnim.ATTACK;
    }

    void SwitchWeapon(float mouseScrollDelta)
    {
        currentWepIndex += (int)mouseScrollDelta;

        if (inventory.Count <= 0) return;

        if (currentWepIndex < 0) currentWepIndex = inventory.Count - 1;
        if (currentWepIndex >= WEAPON_COUNT || currentWepIndex >= inventory.Count) currentWepIndex = 0;

        currentWeapon = inventory[currentWepIndex];
        currentWepScript = currentWeapon.GetComponent<Weapons>();

        switchWepEvent.Invoke();
    }

    void PickUpWeapon()
    {
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, 1.0f, weaponMask);

        if (weaponCollider == null) return;

        GameObject weaponToAdd;
        if (weaponCollider.transform.position.z >= 0.0f)
        {
            weaponToAdd = weaponCollider.gameObject;
        }
        else
        {
            return;
        }

        // Hide weapon added
        weaponToAdd.transform.position = weaponToAdd.transform.position - Vector3.forward * 10;

        if (inventory.Count < WEAPON_COUNT)
        {
            inventory.Add(weaponToAdd);

            if(inventory.Count == 1)
            {
                currentWeapon = weaponToAdd;
                currentWepScript = currentWeapon.GetComponent<Weapons>();
            }
        }
        else
        {
            inventory[currentWepIndex].transform.position = transform.position;
            inventory.RemoveAt(currentWepIndex);
            inventory.Insert(currentWepIndex, weaponToAdd);
        }

        invUpdateEvent.Invoke();

        /*
        for(int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i] == null)
            {
                currentWepNum++;
                AddToInventory(i, weaponToAdd);

                return;
            }
        }

        // Remove and put current weapon at player position then add weapon to inventory
        
        AddToInventory(currentWepIndex, weaponToAdd);
        */
    }

    void AddToInventory(int index, GameObject weaponToAdd)
    {
        inventory[index] = weaponToAdd;
        
        
        invUpdateEvent.Invoke();
    }

    void UpdateInv()
    {
        foreach(GameObject wep in inventory)
        {
            if(wep == null)
            {
                inventory.Remove(wep);

                if(currentWeapon != null)
                {
                    currentWepIndex = inventory.IndexOf(currentWeapon);
                }
                else if (inventory.Count <= 0)
                {
                    currentWepIndex = 0;
                }
                else
                {
                    currentWepIndex = 0;
                    currentWeapon = inventory[currentWepIndex];
                    currentWepScript = currentWeapon.GetComponent<Weapons>();
                }

                break;
            }
        }

        invUpdateEvent.Invoke();
    }

    public Weapons GetCurrentWeapon(){
        if(currentWeapon == null)
        {
            return null;
        }

        if(currentWeapon.GetComponent<Weapons>() == null)
        {
            return null;
        }
        return currentWeapon.GetComponent<Weapons>();
    }
}
