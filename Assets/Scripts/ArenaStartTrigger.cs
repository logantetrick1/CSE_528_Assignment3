using UnityEngine;

public class ArenaStartTrigger : MonoBehaviour
{
    public ArenaManager arenaManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (arenaManager != null)
            {
                arenaManager.StartArena();
            }
        }
    }
}