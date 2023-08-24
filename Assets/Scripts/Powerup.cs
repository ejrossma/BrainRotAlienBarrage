using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum type { speed, invulnerable, doubleShot };
    private Player p;

    [Header("Pickup Information")]
    public type typeOfPickup;
    public float powerupDuration;

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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            p = collider.gameObject.GetComponent<Player>();
            p.ActivatePowerup(this);
            Destroy(gameObject);
        }
    }
}
