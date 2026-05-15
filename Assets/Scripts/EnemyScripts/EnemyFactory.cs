using System.Collections;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Enemy Colors")]
    [SerializeField] private Color[] enemyColors;

    [Header("Material To Recolour")]
    [SerializeField] private Material targetMaterial;

    [Header("Base Wave Settings")]
    [SerializeField] private int baseSpawns = 10;

    [SerializeField] private float spawnRate = 1f;

    [Header("Camera Spawn Settings")]
    [SerializeField] private Camera mainCamera;

    [SerializeField]
    private float maxSpawnDistanceOutsideView = 2f;

    [SerializeField]
    private float spawnYLevel = 0f;

    private int currentTotalSpawns;

    private int spawnedCount;

    private Coroutine spawnRoutine;

    private void Start()
    {
        Time.timeScale = 1f;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        StartWave(1, 0);
    }

    public void StartWave(
        int wave,
        int enemiesAddedPerWave)
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance
                .SetSpawningFinished(false);
        }

        spawnedCount = 0;

        currentTotalSpawns =
            baseSpawns +
            ((wave - 1) * enemiesAddedPerWave);

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }

        spawnRoutine =
            StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (spawnedCount <
               currentTotalSpawns)
        {
            SpawnEnemy();

            spawnedCount++;

            yield return new WaitForSeconds(
                spawnRate
            );
        }

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance
                .SetSpawningFinished(true);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
            return;

        GameObject prefab =
            enemyPrefabs[
                Random.Range(
                    0,
                    enemyPrefabs.Length
                )
            ];

        Vector3 spawnPosition =
            GetSpawnPositionOutsideView();

        GameObject enemy =
            Instantiate(
                prefab,
                spawnPosition,
                Quaternion.identity
            );

        ApplyRandomColor(enemy);
    }

    void ApplyRandomColor(GameObject enemy)
    {
        if (enemyColors.Length == 0)
            return;

        if (targetMaterial == null)
            return;

        Color chosenColor =
            enemyColors[
                Random.Range(
                    0,
                    enemyColors.Length
                )
            ];

        Renderer[] renderers =
            enemy.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials =
                renderer.materials;

            for (int i = 0;
                 i < materials.Length;
                 i++)
            {
                // MATCH MATERIAL
                if (materials[i].name.Contains(
                    targetMaterial.name))
                {
                    // create unique instance
                    materials[i].color =
                        chosenColor;
                }
            }
        }
    }

    Vector3 GetSpawnPositionOutsideView()
    {
        float camHeight =
            mainCamera.orthographicSize;

        float camWidth =
            camHeight * mainCamera.aspect;

        Vector3 camPos =
            mainCamera.transform.position;

        int side = Random.Range(0, 4);

        float x = 0f;
        float z = 0f;

        switch (side)
        {
            case 0:

                x = Random.Range(
                    -camWidth,
                    camWidth
                );

                z = camHeight +
                    Random.Range(
                        0f,
                        maxSpawnDistanceOutsideView
                    );

                break;

            case 1:

                x = Random.Range(
                    -camWidth,
                    camWidth
                );

                z = -camHeight -
                    Random.Range(
                        0f,
                        maxSpawnDistanceOutsideView
                    );

                break;

            case 2:

                x = camWidth +
                    Random.Range(
                        0f,
                        maxSpawnDistanceOutsideView
                    );

                z = Random.Range(
                    -camHeight,
                    camHeight
                );

                break;

            case 3:

                x = -camWidth -
                    Random.Range(
                        0f,
                        maxSpawnDistanceOutsideView
                    );

                z = Random.Range(
                    -camHeight,
                    camHeight
                );

                break;
        }

        return new Vector3(
            camPos.x + x,
            spawnYLevel,
            camPos.z + z
        );
    }
}