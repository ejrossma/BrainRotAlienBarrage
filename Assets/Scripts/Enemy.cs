using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject ammoPickup;
    [SerializeField] GameObject superSpeedPowerup;
    [SerializeField] GameObject invulnerablePowerup;
    [SerializeField] GameObject doubleShotPowerup;
    [SerializeField] Transform frontProjectileLaunchSpot;
    [SerializeField] Transform backProjectileLaunchSpot;
    [SerializeField] Transform leftProjectileLaunchSpot;
    [SerializeField] Transform rightProjectileLaunchSpot;
    [SerializeField] Transform lootSpawnDropSpot;

    private Player player;

    [Header("Enemy Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] float shotCooldown;
    [SerializeField, Range(0,1)] float powerupDropRate;
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
            float t = Random.Range(0f, 1f);
            if (t < powerupDropRate)
            {
                float b = Random.Range(0f, 1f);
                if (b < 0.33f)
                {
                    Instantiate(superSpeedPowerup, lootSpawnDropSpot.position, Quaternion.identity);
                }   
                else if (b < 0.66f)
                {
                    Instantiate(invulnerablePowerup, lootSpawnDropSpot.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(doubleShotPowerup, lootSpawnDropSpot.position, Quaternion.identity);
                }
            }
            else
            {
                GameObject temp = Instantiate(ammoPickup, lootSpawnDropSpot.position, Quaternion.identity);
                temp.GetComponent<Pickup>().SetRandomSize();
            }
            Destroy(gameObject);
        }  
    }
}
