using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] float maxHealth;
    private float health;

    [Header("Player Information")]
    [SerializeField] GameObject currentGun;
    [SerializeField] KeyCode swapGunKey = KeyCode.Q;
    public bool hasControl;

    [Header("Powerups")]
    private List<Powerup.type> activePowerups = new List<Powerup.type>();
    public bool invulnerable;
    private float invulnerableActiveTimer;
    public bool doubleTime;
    private float doubleTimeActiveTimer;
    public bool superSpeed;
    private float superSpeedActiveTimer;

    [Header("Crosshairs")]
    [SerializeField] Sprite assaultCrosshair;
    [SerializeField] Sprite shotgunCrosshair;

    [Header("References")]
    [SerializeField] GameObject assaultObj;
    [SerializeField] GameObject shotgunObj;
    [SerializeField] RectTransform healthAmount;
    [SerializeField] Image currentCrosshair;
    [SerializeField] GameManager gm;


    private void Awake()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthAmount.localScale = new Vector3(health / maxHealth, healthAmount.localScale.y, healthAmount.localScale.z);

        if (Input.GetKeyDown(swapGunKey) && hasControl)
        {
            //check which gun is current
            if (currentGun == assaultObj)
            {
                currentGun = shotgunObj;
                assaultObj.SetActive(false);
                shotgunObj.SetActive(true);
                currentCrosshair.sprite = shotgunCrosshair;
            }
            else
            {
                currentGun = assaultObj;
                shotgunObj.SetActive(false);
                assaultObj.SetActive(true);
                currentCrosshair.sprite = assaultCrosshair;
            }
        }

        ManagePowerups();
    }

    private void ManagePowerups()
    {
        if (activePowerups.Count == 0) return;

        for (int i = activePowerups.Count - 1; i >= 0; i--)
        {
            Powerup.type powerup = activePowerups[i];

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
            else if (powerup == Powerup.type.doubleTime)
            {
                doubleTimeActiveTimer -= Time.deltaTime;
                if (doubleTimeActiveTimer < 0)
                {
                    activePowerups.Remove(powerup);
                    doubleTime = false;
                }
                else
                {
                    doubleTime = true;
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
            activePowerups.Add(powerup.typeOfPickup);

        SetDuration(powerup.typeOfPickup, powerup.powerupDuration);
    }

    private void SetDuration(Powerup.type type, float duration)
    {
        if (type == Powerup.type.speed)
            superSpeedActiveTimer = duration;
        else if (type == Powerup.type.invulnerable)
            invulnerableActiveTimer = duration;
        else if (type == Powerup.type.doubleTime)
            doubleTimeActiveTimer = duration;
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
            health += 50;
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
        if (!invulnerable && GetComponent<PlayerMovement>().GetMovementState() != PlayerMovement.MovementState.superarmor)
            health -= damage;
        if (health <= 0)
        {
            gm.Lose();
            health = 0;
        }
            
    }
}
