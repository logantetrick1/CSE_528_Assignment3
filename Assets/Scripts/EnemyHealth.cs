using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    private ArenaManager arenaManager;
    [Header("Health")]
    public int maxHealth = 30;
    private int currentHealth;

    [Header("Death")]
    public float destroyDelay = 3f;

    private bool isDead = false;

    private Animator animator;
    private NavMeshAgent agent;
    private Collider enemyCollider;

    private SkeletonAI skeletonAI;
    private WizardAI wizardAI;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<Collider>();
        arenaManager = FindObjectOfType<ArenaManager>();
        // get whichever AI exists
        skeletonAI = GetComponent<SkeletonAI>();
        wizardAI = GetComponent<WizardAI>();
        arenaManager = FindObjectOfType<ArenaManager>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log(gameObject.name + " took damage: " + damage);
        Debug.Log("EnemyHealth found on hit object or parent");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Play death animation
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        // Stop movement
        if (agent != null)
        {
            agent.isStopped = true;
        }

        // Stop AI scripts
        if (skeletonAI != null)
        {
            skeletonAI.SetAlive(false);
        }

        if (wizardAI != null)
        {
            wizardAI.SetAlive(false);
        }
        
        if (arenaManager != null)
        {
            arenaManager.EnemyDefeated();
        }

        // Destroy after delay
        Destroy(gameObject, destroyDelay);
    }
}