using UnityEngine;

public class WizardAI : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRange = 25f;
    public float eyeHeight = 1.5f;
    public float facingThreshold = 0.3f;

    [Header("Scanning")]
    public float scanAngle = 60f;
    public float scanSpeed = 30f;

    [Header("Attack")]
    public float attackCooldown = 2f;
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float projectileSpeed = 18f;

    [Header("Animation")]
    public Animator animator;

    private Transform playerTransform;
    private bool _alive = true;
    private float attackTimer = 0f;

    private float startYRotation;
    private bool scanningRight = true;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        startYRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        if (!_alive) return;
        if (playerTransform == null) return;

        attackTimer += Time.deltaTime;

        if (CanSeePlayer())
        {
            FacePlayer();

            if (attackTimer >= attackCooldown)
            {
                attackTimer = 0f;

                if (animator != null)
                {
                    animator.SetTrigger("attackTrigger");
                }

                ShootAtPlayer();
            }
        }
        else
        {
            ScanSideToSide();
        }
    }

    bool CanSeePlayer()
    {
        if (playerTransform == null) return false;

        Vector3 wizardEyePos = transform.position + Vector3.up * eyeHeight;
        Vector3 playerTargetPos = playerTransform.position + Vector3.up * 1.0f;

        float distanceToPlayer = Vector3.Distance(wizardEyePos, playerTargetPos);
        if (distanceToPlayer > detectionRange)
        {
            return false;
        }

        Vector3 directionToPlayer = (playerTargetPos - wizardEyePos).normalized;
        float dot = Vector3.Dot(transform.forward, directionToPlayer);

        return dot > facingThreshold;
    }

    void FacePlayer()
    {
        Vector3 lookTarget = new Vector3(
            playerTransform.position.x,
            transform.position.y,
            playerTransform.position.z
        );

        transform.LookAt(lookTarget);
    }

    void ScanSideToSide()
    {
        float minY = startYRotation - scanAngle;
        float maxY = startYRotation + scanAngle;

        float currentY = NormalizeAngle(transform.eulerAngles.y);
        float direction = scanningRight ? 1f : -1f;
        float nextY = currentY + direction * scanSpeed * Time.deltaTime;

        if (nextY >= maxY)
        {
            nextY = maxY;
            scanningRight = false;
        }
        else if (nextY <= minY)
        {
            nextY = minY;
            scanningRight = true;
        }

        transform.rotation = Quaternion.Euler(0f, nextY, 0f);
    }

    float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }

    void ShootAtPlayer()
    {
        if (fireballPrefab == null || playerTransform == null) return;

        Vector3 spawnPos = firePoint != null
            ? firePoint.position
            : transform.position + transform.forward * 1.5f + Vector3.up * 1f;

        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 target = playerTransform.position + Vector3.up * 1.0f;
            Vector3 shootDirection = (target - spawnPos).normalized;
            rb.linearVelocity = shootDirection * projectileSpeed;
        }
    }

    public void SetAlive(bool alive)
    {
        _alive = alive;

        if (!_alive && animator != null)
        {
            animator.SetBool("isDead", true);
        }
    }

    public bool IsAlive()
    {
        return _alive;
    }
}