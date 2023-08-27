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
    private bool parried;

    private void FixedUpdate()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    public void SetMoveDir(Vector3 moveDirection)
    {
        moveDir = moveDirection;
    }

    public Vector3 GetMoveDir() { return moveDir; }

    public void GotDeflected()
    {
        parried = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( (collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Enemy")) && !parried) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponentInParent<PlayerMovement>().GetMovementState() == PlayerMovement.MovementState.superarmor)
            {
                GotDeflected();
                Vector3 temp = Vector3.Reflect(GetMoveDir(), collision.GetContact(0).normal);
                Vector3 final = (collision.gameObject.GetComponentInParent<PlayerMovement>().GetOrientation().forward + temp) / 2;

                SetMoveDir(new Vector3(final.x, 0, final.z));
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
