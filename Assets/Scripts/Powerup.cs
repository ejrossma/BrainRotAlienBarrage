using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum type { speed, invulnerable, doubleTime };
    private Player p;

    [Header("Pickup Information")]
    public type typeOfPickup;
    public float powerupDuration;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            p = collider.gameObject.GetComponentInParent<Player>();
            p.ActivatePowerup(this);
            Destroy(gameObject);
        }
    }
}
