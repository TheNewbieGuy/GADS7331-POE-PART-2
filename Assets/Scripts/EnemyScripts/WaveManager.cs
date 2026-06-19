using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("References")]
    [SerializeField] private EnemyFactory enemyFactory;

    [SerializeField] private Transform player;

    [Header("Player Reset")]
    [SerializeField]
    private Vector3 playerResetPosition =
        new Vector3(0f, 5f, 0f);

    [Header("Wave Settings")]
    [SerializeField]
    private int currentWave = 1;

    [SerializeField]
    private int enemiesAddedPerWave = 5;

    private void Awake()
    {
        Instance = this;
    }

    public void StartNextWave()
    {
        currentWave++;

        Debug.Log(
            "STARTING WAVE: " +
            currentWave
        );

        Time.timeScale = 1f;

        if (player != null)
        {
            player.position =
                playerResetPosition;
            PlayerStats stats =
                player.GetComponent<PlayerStats>();

            if (stats != null)
            {
                stats.ResetStatsForNewWave();
            }

            Rigidbody rb =
                player.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity =
                    Vector3.zero;
            }
        }

        if (EndSequenceUI.Instance != null)
        {
            EndSequenceUI.Instance
                .HideEndUI();
        }

        if (enemyFactory != null)
        {
            enemyFactory.StartWave(
                currentWave,
                enemiesAddedPerWave
            );
        }
    }
}