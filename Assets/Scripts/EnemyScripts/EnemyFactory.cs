using System.Collections;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private EnemyModifier currentModifier =
        EnemyModifier.None;
    private EnemyModifier lastModifier =
        EnemyModifier.None;
    
    
    [Header("Enemies")]
    [SerializeField] private GameObject[] enemyPrefabs;

    private PlayerStats player;

    [Header("Powerup Component")]
    [SerializeField] private bool usePowerups = true;

    [Header("Base Wave Settings")]
    [SerializeField] private int baseSpawns = 10;
    [SerializeField] private float spawnRate = 1f;

    [Header("Camera Spawn Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxSpawnDistanceOutsideView = 2f;
    [SerializeField] private float spawnYLevel = 0f;

    private int currentTotalSpawns;
    private int spawnedCount;

    private Coroutine spawnRoutine;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerStats>();

        Time.timeScale = 1f;

        if (mainCamera == null)
            mainCamera = Camera.main;

        StartWave(1, 0);
    }

    public void StartWave(int wave, int enemiesAddedPerWave)
    {
        if (EnemyManager.Instance != null)
            EnemyManager.Instance.SetSpawningFinished(false);

        spawnedCount = 0;

        currentTotalSpawns =
            baseSpawns + ((wave - 1) * enemiesAddedPerWave);

        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        spawnRoutine =
            StartCoroutine(StartWaveRoutine());
    }
    private IEnumerator StartWaveRoutine()
    {
        yield return PrepareWaveModifier();

        yield return SpawnRoutine();
    }

    IEnumerator SpawnRoutine()
    {
        while (spawnedCount < currentTotalSpawns)
        {
            SpawnEnemy();
            spawnedCount++;

            float currentSpawnRate = 2f;

            if (player != null)
            {
                currentSpawnRate =
                    2f - (0.2f * player.GetDamage());
            }

            currentSpawnRate =
                Mathf.Clamp(currentSpawnRate, 0.25f, 2f);

            yield return new WaitForSeconds(currentSpawnRate);
        }

        EnemyManager.Instance?.SetSpawningFinished(true);
    }
    private void ApplyWaveModifier(
        EnemyBase enemy
    )
    {
        switch (currentModifier)
        {
            case EnemyModifier.Fast:

                enemy.SetMoveSpeed(
                    enemy.GetMoveSpeed() + 3f
                );

                break;

            case EnemyModifier.Tanky:

                enemy.SetHealth(
                    Mathf.RoundToInt(
                        enemy.GetHealth() + 3f
                    )
                );

                break;

            case EnemyModifier.Strong:

                enemy.SetDamage(
                    Mathf.RoundToInt(
                        enemy.GetDamage() + 25f
                    )
                );

                break;
        }
    }
    
    public IEnumerator PrepareWaveModifier()
    {
        if (player == null)
            yield break;

        bool finished = false;

        yield return OllamaManager.Instance
            .GetWaveModifier(
                player,
                lastModifier,
                (modifier) =>
                {
                    currentModifier =
                        modifier;

                    lastModifier =
                        modifier;

                    finished = true;
                }
            );

        while (!finished)
            yield return null;

        Debug.Log(
            "Wave Modifier: " +
            currentModifier
        );
    }
    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
            return;

        GameObject prefab =
            enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Vector3 spawnPosition = GetSpawnPositionOutsideView();

        GameObject enemyObj =
            Instantiate(prefab, spawnPosition, Quaternion.identity);

        EnemyBase enemy = enemyObj.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            ApplyWaveModifier(enemy);
        }

        if (usePowerups)
        {
            EnemyPowerup powerup =
                enemyObj.GetComponent<EnemyPowerup>();

            if (powerup != null)
                powerup.AssignRandomPowerup();
        }
    }

   

    Vector3 GetSpawnPositionOutsideView()
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        Vector3 camPos = mainCamera.transform.position;

        int side = Random.Range(0, 4);

        float x = 0f;
        float z = 0f;

        switch (side)
        {
            case 0:
                x = Random.Range(-camWidth, camWidth);
                z = camHeight + Random.Range(0f, maxSpawnDistanceOutsideView);
                break;

            case 1:
                x = Random.Range(-camWidth, camWidth);
                z = -camHeight - Random.Range(0f, maxSpawnDistanceOutsideView);
                break;

            case 2:
                x = camWidth + Random.Range(0f, maxSpawnDistanceOutsideView);
                z = Random.Range(-camHeight, camHeight);
                break;

            case 3:
                x = -camWidth - Random.Range(0f, maxSpawnDistanceOutsideView);
                z = Random.Range(-camHeight, camHeight);
                break;
        }

        return new Vector3(camPos.x + x, spawnYLevel, camPos.z + z);
    }
}