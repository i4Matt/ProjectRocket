using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{

    [Header("References")]
    public Transform player;
    public Transform attackPoint;
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject projectile;

    [Header("Settings")]
    //Attacking
    public float attackRange;
    public float bulletSpeed;
    public float timeBetweenAttacks;

    //States
    private bool alreadyAttacked;
    private bool playerInAttackRange;

    [Header("Audio Sources")]
    public AudioSource shootingSFX;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //check for sight and attack range
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

       // if (!playerInSightRange && !playerInAttackRange) Patroling();
        // if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange) AttackPlayer();
    }

    void Patroling()
    {
        /*if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkpoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkpoint;

        //Walkpoint Reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }*/
    }

    void SearchWalkPoint()
    {
        /*float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkpoint = new Vector3 (transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }*/
    }
    void ChasePlayer()
    {
        //agent.SetDestination(player.position);
    }
    void AttackPlayer()
    {
        //Make sure enemy doesn't move
        //agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            shootingSFX.Play();
            //Attack Code
            Rigidbody shot= Instantiate(projectile, attackPoint.position, Quaternion.identity).GetComponent<Rigidbody>();

            shot.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

}
