using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsProjectile : MonoBehaviour
{
    [Header("Projectile Stats")]
    [SerializeField] float damage;

    [Header("Movement")]
    private bool parried;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GotDeflected()
    {
        parried = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Enemy")) && !parried) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponentInParent<PlayerMovement>().GetMovementState() == PlayerMovement.MovementState.superarmor)
            {
                GotDeflected();
                rb.velocity = Vector3.Reflect(rb.velocity, collision.GetContact(0).normal);
                rb.AddForce(-transform.forward * 28f, ForceMode.Impulse);
                rb.AddForce(transform.up * 3f, ForceMode.Impulse);
                return;
            }
            else
            {
                collision.gameObject.GetComponentInParent<Player>().TakeDamage(damage);
            }
        }
        else if (parried && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}

