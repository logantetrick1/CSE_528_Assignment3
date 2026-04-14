using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{
    public float speed = 10.0f;
    public int damage = 10;

    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}