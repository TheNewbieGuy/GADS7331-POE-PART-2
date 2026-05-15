using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private PlayerStats player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerStats>();

        if (player == null)
        {
            Debug.LogError("PlayerStats not found");
            return;
        }

        slider.value = 1f;
    }

    private void Update()
    {
        if (player == null) return;

        float normalizedHealth =
            (float)player.GetCurrentHealth() /
            player.GetMaxHealth();

        slider.value = normalizedHealth;
    }
}