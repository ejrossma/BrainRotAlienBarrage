using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float startingHealth;
    private float health;
    [Range(0,1)] public float powerupDropRate;
    

    [Header("References")]
    [SerializeField] GameObject ammoPickup;
    [SerializeField] GameObject superSpeedPowerup;
    [SerializeField] GameObject invulnerablePowerup;
    [SerializeField] GameObject doubleShotPowerup;
    [SerializeField] Transform lootSpawnDropSpot;
    private GameManager gm;

    private void Awake()
    {
        health = startingHealth;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
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

    private void ResetEnemy()
    {
        health = startingHealth;
    }
}
