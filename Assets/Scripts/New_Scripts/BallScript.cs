using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
    public int damage = 10;

    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    void OnTriggerEnter(Collider other)
    {
        // Ignore player
        if (other.GetComponentInParent<PlayerHealth>() != null)
            return;

        // Damage enemies
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Destroy on anything else
        Destroy(gameObject);
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}