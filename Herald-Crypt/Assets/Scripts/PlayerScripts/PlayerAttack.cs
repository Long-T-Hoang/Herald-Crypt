using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;

    [Header("Weapon switch and inventory")]
    [SerializeField]
    private GameObject[] inventory;
    private GameObject currentWeapon;
    private Weapons currentWepScript;
    public int currentWepIndex;
    private const int WEAPON_COUNT = 3;

    // Attack cooldown
    [SerializeField]
    private float attackCooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentWepIndex = 0;

        attackCooldownTimer = 0.0f;
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

        if (currentWepIndex < 0) currentWepIndex = WEAPON_COUNT - 1;
        if (currentWepIndex >= WEAPON_COUNT) currentWepIndex = 0;

        currentWeapon = inventory[currentWepIndex];
        currentWepScript = currentWeapon.GetComponent<Weapons>();
    }
}
