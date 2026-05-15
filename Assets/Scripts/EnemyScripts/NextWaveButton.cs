using UnityEngine;

public class NextWaveButton : MonoBehaviour
{
    public void StartNextWave()
    {
        PlayerStats player =
            FindFirstObjectByType<PlayerStats>();

        if (player != null)
        {
            player.ResetStatsForNewWave();
        }

        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.StartNextWave();
        }
    }
}