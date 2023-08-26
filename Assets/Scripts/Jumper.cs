using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jumper : MonoBehaviour
{
    [Header("References")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask groundLayer, playerLayer;

    //activate enemy
    public bool triggeredToFight;

    [Header("Attacking")]
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float attackRange;
    [SerializeField] bool playerInAttackRange;
    bool alreadyAttacked;

    [Header("Jump Attack")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float depletionCoefficient;
    private float depletingSpeed;
    [SerializeField] float jumpDamage;
    
    private float startJumpDist;
    private Vector3 targetPosition;
    private float lastDistanceFromTarget;
    private bool jumping;
    private Vector3 jumpDir;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (jumping)
        {
            Jump();
        }
        else if (triggeredToFight && !playerInAttackRange && !alreadyAttacked)
        {
            ResetIdle();
            ChasePlayer();
        }
        else if (triggeredToFight && playerInAttackRange && !alreadyAttacked)
        {
            ResetIdle();
            AttackPlayer();
        }
        else if (triggeredToFight && alreadyAttacked) Idle();

        
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            jumping = true;
            animator.SetBool("Jumping", jumping);
            depletingSpeed = jumpSpeed;
            lastDistanceFromTarget = 1000;
            jumpDir = ((player.position + new Vector3(0, -0.5f, 0)) - transform.position).normalized;
            targetPosition = player.position;
            agent.enabled = false;
            alreadyAttacked = true;

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void Jump()
    {
        depletingSpeed -= jumpSpeed / depletionCoefficient;

        transform.position += jumpDir * depletingSpeed * Time.deltaTime;

        float distanceFromTarget = Vector3.Distance(transform.position, targetPosition);

        if (lastDistanceFromTarget < distanceFromTarget && distanceFromTarget > 7.5f)
            ResetJump();

        lastDistanceFromTarget = distanceFromTarget;
    }

    private void Idle()
    {
        animator.SetBool("Idle", true);
    }

    private void ResetIdle()
    {
        animator.SetBool("Idle", false);
    }

    private void ResetJump()
    {
        jumping = false;
        animator.SetBool("Jumping", jumping);
        agent.enabled = true;
    }
    
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void SetTriggeredToFight(bool triggered)
    {
        triggeredToFight = triggered;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && jumping)
        {
            collision.gameObject.GetComponentInParent<Player>().TakeDamage(jumpDamage);
            ResetJump();
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground"))
        {
            ResetJump();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
