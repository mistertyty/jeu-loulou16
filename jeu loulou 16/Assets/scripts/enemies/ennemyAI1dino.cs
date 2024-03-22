
using UnityEngine;
using UnityEngine.AI;

public class ennemyAIdino : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private Vector3 destination;
    public ParticleSystem deathEffect;
    public dino dino;
    public pauseTrigger pausetrigger;
    public int value;
    private Vector3 prevPos;
    public GameObject parent;

    private void Awake()
    {
        prevPos = gameObject.transform.position;
        pausetrigger = FindObjectOfType<pauseTrigger>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        Patroling();

        transform.LookAt(destination);

        if (dino.health <= 0)
            DestroyEnemy();

        if (gameObject.transform.position == prevPos)
            walkPointSet = false;
        prevPos = gameObject.transform.position;
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
        destination = walkPoint;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    private void DestroyEnemy()
    {
        Debug.Log("i get 1 point yay");
        pausetrigger.score += value;
        Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(parent);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
