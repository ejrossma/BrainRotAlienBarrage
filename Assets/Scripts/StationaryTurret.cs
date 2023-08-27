using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class StationaryTurret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject projectile;
    [SerializeField] Transform frontProjectileLaunchSpot;
    [SerializeField] Transform backProjectileLaunchSpot;
    [SerializeField] Transform leftProjectileLaunchSpot;
    [SerializeField] Transform rightProjectileLaunchSpot;

    [Header("Projectiles")]
    [SerializeField] float shotCooldown;
    private float shotCooldownTimer;

    private Enemy self;

    void Awake()
    {
        self = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
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
}
