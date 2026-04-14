using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ArenaManager : MonoBehaviour
{
    [Header("Arena")]
    public GameObject gateObject;
    public GameObject arenaStartTrigger;
    public bool arenaStarted = false;
    public AudioClip victorySound;
    private AudioSource audioSource;
    [Header("Win Objects")]
    public GameObject playerCrownObject;
    public GameObject winTextObject;

    [Header("UI")]
    public TMP_Text enemyCountText;
    public TMP_Text playerHealthText;

    [Header("Enemies")]
    public int totalEnemies = 0;
    private int remainingEnemies = 0;

    [Header("Player")]
    public PlayerHealth playerHealth;

    void Start()
    {
        remainingEnemies = totalEnemies;
        audioSource = GetComponent<AudioSource>();
        if (playerCrownObject != null)
            playerCrownObject.SetActive(false);

        if (winTextObject != null)
            winTextObject.SetActive(false);

        UpdateEnemyUI();
        UpdatePlayerHealthUI();
    }

    void Update()
    {
        UpdatePlayerHealthUI();
    }

    public void StartArena()
    {
        if (arenaStarted) return;

        arenaStarted = true;

        if (gateObject != null)
            gateObject.SetActive(true);

        if (arenaStartTrigger != null)
            arenaStartTrigger.SetActive(false);
    }

    public void EnemyDefeated()
    {
        remainingEnemies--;
        if (remainingEnemies < 0)
            remainingEnemies = 0;

        UpdateEnemyUI();

        if (remainingEnemies == 0)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        if (playerCrownObject != null)
            playerCrownObject.SetActive(true);
        audioSource.PlayOneShot(victorySound);
        if (winTextObject != null)
            winTextObject.SetActive(true);
    }

    void UpdateEnemyUI()
    {
        if (enemyCountText != null)
            enemyCountText.text = "Enemies Left: " + remainingEnemies;
    }

    void UpdatePlayerHealthUI()
    {
        if (playerHealthText != null && playerHealth != null)
            playerHealthText.text = "Health: " + playerHealth.GetCurrentHealth();
    }
}