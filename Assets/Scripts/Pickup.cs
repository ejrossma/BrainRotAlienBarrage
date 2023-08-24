using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum size { small, medium, full };
    public enum type { ammo, health };

    [Header("Pickup Information")]
    [SerializeField] size sizeOfPickup;
    [SerializeField] type typeOfPickup;

    public void SetRandomSize()
    {
        float val = Random.Range(0f, 1f);
        if (val > 0.33f)
            sizeOfPickup = size.small;
        else
            sizeOfPickup = size.medium;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Player p = collider.gameObject.GetComponentInParent<Player>();

            bool pickUpUsed = false;

            if (typeOfPickup == type.ammo)
                pickUpUsed = p.AddAmmo(sizeOfPickup);
            else if (typeOfPickup == type.health)
                pickUpUsed = p.AddHealth(sizeOfPickup);

            if (pickUpUsed) Destroy(gameObject);
        }
    }
}
