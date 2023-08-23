using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Stats")]
    [SerializeField] float speed;
    [SerializeField] float damage;

    [Header("Movement")]
    private Vector3 moveDir;

    private void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    public void SetMoveDir(Vector3 moveDirection)
    {
        moveDir = moveDirection;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Projectile") || collider.gameObject.CompareTag("Enemy")) return;

        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponentInParent<Player>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
