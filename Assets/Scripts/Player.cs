using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] float health;

    [Header("Player Information")]
    [SerializeField] GameObject currentGun;
    [SerializeField] KeyCode swapGunKey = KeyCode.Q;

    [Header("Powerups")]
    private List<Powerup.type> activePowerups = new List<Powerup.type>();
    [SerializeField] bool invulnerable;
    private float invulnerableActiveTimer;
    [SerializeField] bool doubleShot;
    private float doubleShotActiveTimer;
    [SerializeField] bool superSpeed;
    private float superSpeedActiveTimer;

    [Header("References")]
    [SerializeField] GameObject assaultObj;
    [SerializeField] GameObject shotgunObj;
    [SerializeField] TextMeshProUGUI healthText;

    // Update is called once per frame
    void Update()
    {
        healthText.SetText("Health: " + health);

        if (Input.GetKeyDown(swapGunKey))
        {
            //check which gun is current
            if (currentGun == assaultObj)
            {
                currentGun = shotgunObj;
                assaultObj.SetActive(false);
                shotgunObj.SetActive(true);
            }
            else
            {
                currentGun = assaultObj;
                shotgunObj.SetActive(false);
                assaultObj.SetActive(true);
            }
        }

        ManagePowerups();
    }

    private void ManagePowerups()
    {
        foreach (Powerup.type powerup in activePowerups)
        {
            if (powerup == Powerup.type.speed)
            {
                superSpeedActiveTimer -= Time.deltaTime;
                if (superSpeedActiveTimer < 0)
                {
                    activePowerups.Remove(powerup);
                    superSpeed = false;
                }
                else
                {
                    superSpeed = true;
                }
            }
            else if (powerup == Powerup.type.doubleShot)
            {
                doubleShotActiveTimer -= Time.deltaTime;
                if (doubleShotActiveTimer < 0)
                {
                    activePowerups.Remove(powerup);
                    doubleShot = false;
                }
                else
                {
                    doubleShot = true;
                }    
            }
            else if (powerup == Powerup.type.invulnerable)
            {
                invulnerableActiveTimer -= Time.deltaTime;
                if (invulnerableActiveTimer < 0)
                {
                    activePowerups.Remove(powerup);
                    invulnerable = false;
                }
                else
                {
                    invulnerable = true;
                }
            }
        }
    }

    public void ActivatePowerup(Powerup powerup)
    {
        if (!activePowerups.Contains(powerup.typeOfPickup))
        {
            activePowerups.Add(powerup.typeOfPickup);
        }

        SetDuration(powerup.typeOfPickup, powerup.powerupDuration);
    }

    private void SetDuration(Powerup.type type, float duration)
    {
        if (type == Powerup.type.speed)
            superSpeedActiveTimer = duration;
    }

    public bool AddAmmo(Pickup.size sizeOfPickup)
    {
        GunSystem gs = currentGun.GetComponent<GunSystem>();

        if (gs.IsAmmoFull())
            return false;

        int ammoMult = 1;
        if (currentGun == assaultObj)
            ammoMult = 5;

        if (sizeOfPickup == Pickup.size.small)
        {
            gs.AddAmmo(2 * ammoMult);
        }
        else if (sizeOfPickup == Pickup.size.medium)
        {
            gs.AddAmmo(3 * ammoMult);
        }
        else if (sizeOfPickup == Pickup.size.full)
        {
            gs.AddAmmo(20 * ammoMult);
        }

        return true;
    }

    public bool AddHealth(Pickup.size sizeOfPickup) 
    {
        if (health == maxHealth) return false;

        if (sizeOfPickup == Pickup.size.small)
        {
            health += 25;
            if (health > maxHealth) health = maxHealth;
        }
        else if (sizeOfPickup == Pickup.size.medium)
        {
            health += 100;
            if (health > maxHealth) health = maxHealth;
        }
        else if (sizeOfPickup == Pickup.size.full)
        {
            health += maxHealth;
            if (health > maxHealth) health = maxHealth;
        }

        return true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
            Debug.Log("You have died");
    }
}
