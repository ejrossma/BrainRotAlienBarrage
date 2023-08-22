using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] float health;

    [Header("Player Information")]
    [SerializeField] GameObject currentGun;
    [SerializeField] KeyCode swapGunKey = KeyCode.Q;

    [Header("Gun References")]
    [SerializeField] GameObject assaultObj;
    [SerializeField] GameObject shotgunObj;

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
            currentGun.GetComponent<GunSystem>().AddAmmo(1 * ammoMult);
        }
        else if (sizeOfPickup == Pickup.size.medium)
        {
            currentGun.GetComponent<GunSystem>().AddAmmo(3 * ammoMult);
        }
        else if (sizeOfPickup == Pickup.size.full)
        {
            currentGun.GetComponent<GunSystem>().AddAmmo(20 * ammoMult);
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
}
