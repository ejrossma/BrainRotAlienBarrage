using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject ammoPickup;
    [SerializeField] Transform frontProjectileLaunchSpot;
    [SerializeField] Transform backProjectileLaunchSpot;
    [SerializeField] Transform leftProjectileLaunchSpot;
    [SerializeField] Transform rightProjectileLaunchSpot;

    private Player player;

    [Header("Enemy Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] float shotCooldown;
    private float shotCooldownTimer;


    [Header("Enemy Information")]
    private float health;

    private void Awake()
    {
        player = GetComponent<Player>();
        health = maxHealth;
        shotCooldownTimer = 0;
    }

    private void Update()
    {
        shotCooldownTimer += Time.deltaTime;
        if (shotCooldownTimer > shotCooldown)
        {
            shotCooldownTimer = 0;
            ShootProjectiles();
        }
    }

    private void ShootProjectiles()
    {
        GameObject temp;
        temp = Instantiate(projectile, frontProjectileLaunchSpot.position, Quaternion.identity);
        temp.GetComponent<Projectile>().SetMoveDir(transform.forward);

        temp = Instantiate(projectile, backProjectileLaunchSpot.position, Quaternion.identity);
        temp.GetComponent<Projectile>().SetMoveDir(-transform.forward);

        temp = Instantiate(projectile, rightProjectileLaunchSpot.position, Quaternion.identity);
        temp.GetComponent<Projectile>().SetMoveDir(transform.right);

        temp = Instantiate(projectile, leftProjectileLaunchSpot.position, Quaternion.identity);
        temp.GetComponent<Projectile>().SetMoveDir(-transform.right);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
            GameObject temp = Instantiate(ammoPickup, transform.position, Quaternion.identity);
            temp.GetComponent<Pickup>().SetRandomSize();
            Destroy(gameObject);
        }  
    }
}
