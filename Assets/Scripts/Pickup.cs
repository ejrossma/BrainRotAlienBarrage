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

    [Header("Pickup Movement")]
    private Vector3 initialPosition;
    [SerializeField] float bobSpeed;
    [SerializeField] float bobHeight;


    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

        float newY = initialPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

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
