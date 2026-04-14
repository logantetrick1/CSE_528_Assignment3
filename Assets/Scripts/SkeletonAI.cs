using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 4.0f;
    public float chaseRange = 10.0f;
    public float attackRange = 2.0f;
    public float wanderRadius = 6.0f;
    public float wanderInterval = 3.0f;

    [Header("Attack")]
    public int damage = 10;
    public float attackCooldown = 1.0f;

    [Header("References")]
    public Animator animator;
    public PlayerHealth playerHealth;

    private NavMeshAgent agent;
    private Transform playerTransform;

    private bool _alive;
    private bool isAttacking = false;
    private bool isMoving = false;

    private float attackTimer;
    private float wanderTimer;
    private Vector3 startPosition;

    void Start()
    {
        _alive = true;
        attackTimer = attackCooldown;
        wanderTimer = wanderInterval;
        startPosition = transform.position;

        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = speed;
            agent.stoppingDistance = attackRange;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }
    }

    void Update()
    {
        if (!_alive) return;
        if (agent == null) return;

        attackTimer += Time.deltaTime;
        wanderTimer += Time.deltaTime;

        isAttacking = false;
        isMoving = false;

        if (playerTransform == null)
        {
            Wander();
            UpdateAnimator();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Wander();
        }

        UpdateAnimator();
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(playerTransform.position);

        isMoving = true;
        isAttacking = false;
    }

    void AttackPlayer()
    {
        agent.isStopped = true;
        isAttacking = true;
        isMoving = false;

        Vector3 lookTarget = new Vector3(
            playerTransform.position.x,
            transform.position.y,
            playerTransform.position.z
        );
        transform.LookAt(lookTarget);

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            if (animator != null)
            {
                animator.SetTrigger("triggerAttack");
            }

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    void Wander()
    {
        if (wanderTimer >= wanderInterval || agent.remainingDistance <= 0.5f)
        {
            Vector3 newPos = RandomNavSphere(startPosition, wanderRadius, -1);
            agent.isStopped = false;
            agent.SetDestination(newPos);
            wanderTimer = 0f;
        }

        isMoving = true;
        isAttacking = false;
    }

    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }
    }

    public void SetAlive(bool alive)
    {
        _alive = alive;

        if (!_alive && agent != null)
        {
            agent.isStopped = true;
        }
    }

    public bool IsAlive()
    {
        return _alive;
    }
}