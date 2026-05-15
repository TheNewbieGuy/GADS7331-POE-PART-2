using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    
    private bool spawningFinished;
    public static EnemyManager Instance;
    [Header("Last Enemy Teleport")]
    [SerializeField]
    private Transform lastEnemyTeleportPoint;
    private List<EnemyBase> aliveEnemies =
        new List<EnemyBase>();

    private List<string> deathFortunes =
        new List<string>();

    private bool resolvingDeaths;

    private void Awake()
    {
        Instance = this;
    }

    public void Register(EnemyBase enemy)
    {
        if (!aliveEnemies.Contains(enemy))
        {
            aliveEnemies.Add(enemy);
        }
    }
    public void SetSpawningFinished(bool finished)
    {
        spawningFinished = finished;
    }
    public bool IsLastEnemy(EnemyBase enemy)
    {
        return spawningFinished &&
               aliveEnemies.Count == 1 &&
               aliveEnemies.Contains(enemy);
    }

    private IEnumerator<WaitForEndOfFrame>
        ResolveDeathsNextFrame()
    {
        resolvingDeaths = true;

        yield return new WaitForEndOfFrame();

        if (aliveEnemies.Count == 0 &&
            deathFortunes.Count > 0)
        {
            string chosenFortune =
                deathFortunes[
                    Random.Range(
                        0,
                        deathFortunes.Count
                    )
                ];

            OnLastEnemyDied(chosenFortune);
        }

        deathFortunes.Clear();

        resolvingDeaths = false;
    }

    private void OnLastEnemyDied(
        string fortune)
    {
        Debug.Log(
            "LAST ENEMY GROUP DIED"
        );

        Debug.Log(
            "SELECTED FORTUNE: " +
            fortune
        );


        if (EndSequenceUI.Instance != null)
        {
            EndSequenceUI.Instance
                .PlayEndSequence(fortune);
        }
    }
    public void HandleEnemyDeath(EnemyBase enemy)
    {
        if (IsLastEnemy(enemy))
        {
            HandleLastEnemy(enemy);
        }
        else
        {
            aliveEnemies.Remove(enemy);

            Destroy(enemy.gameObject);
        }
    }
    private void HandleLastEnemy(EnemyBase enemy)
    {
        aliveEnemies.Remove(enemy);

        if (lastEnemyTeleportPoint != null)
        {
            enemy.transform.position =
                lastEnemyTeleportPoint.position;
        }

        FortuneCookieHalf[] cookieHalves =
            enemy.GetComponentsInChildren<
                FortuneCookieHalf>();

        foreach (FortuneCookieHalf half in cookieHalves)
        {
            half.OpenCookie();
        }

        MonoBehaviour[] scripts =
            enemy.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }

        Rigidbody rb =
            enemy.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }

        Debug.Log(
            "LAST ENEMY FORTUNE: " +
            enemy.fortune
        );

        Time.timeScale = 0f;

        if (EndSequenceUI.Instance != null)
        {
            EndSequenceUI.Instance
                .PlayEndSequence(enemy.fortune);
        }
    }
}