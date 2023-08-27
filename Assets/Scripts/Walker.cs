using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] LayerMask groundLayer, playerLayer;

    [SerializeField] GameObject attackProjectile;
    [SerializeField] Transform launchProjectileSpot;

    [Header("Stationary")]
    [SerializeField] float stationaryRangeMultiplier;
    [SerializeField] bool stationary;

    //activate enemy
    public bool triggeredToFight;

    [Header("Attacking")]
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float attackRange;
    [SerializeField] bool playerInAttackRange;
    bool alreadyAttacked;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (stationary)
        {
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange * stationaryRangeMultiplier, playerLayer);
            if (triggeredToFight && playerInAttackRange) AttackPlayer();
        }
        else
        {
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
            if (triggeredToFight && !playerInAttackRange) ChasePlayer();
            else if (triggeredToFight && playerInAttackRange) AttackPlayer();
        }
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
            //Attack Code
            GameObject tmp = Instantiate(attackProjectile, launchProjectileSpot.position, Quaternion.identity);
            Rigidbody rb = tmp.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 25f, ForceMode.Impulse);
            rb.AddForce(transform.up * 1.5f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void SetTriggeredToFight(bool triggered)
    {
        triggeredToFight = triggered;
    }

    public void SetStationary()
    {
        stationary = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
