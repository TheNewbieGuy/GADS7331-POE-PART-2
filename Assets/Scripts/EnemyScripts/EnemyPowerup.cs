using UnityEngine;

public class EnemyPowerup : MonoBehaviour
{
    public enum PowerUpType
    {
        SpeedBoost,
        DamageBoost,
        HealthBoost,
        Weakness
    }

    [Header("Assigned Powerup (hidden until last enemy)")]
    public PowerUpType powerUp;

    [Header("Powerup Values (TUNABLE)")]
    public int healthChange = 10;
    public int damageChange = 5;
    public float speedChange = 1f;

    

    [Header("Renderers To Colour")]
    [SerializeField] private Renderer[] colouredRenderers;

    public void AssignRandomPowerup()
    {
        PowerUpType[] values =
            (PowerUpType[])System.Enum.GetValues(typeof(PowerUpType));

        powerUp = values[
            Random.Range(0, values.Length)
        ];

        ApplyColour(); 
    }

    private void ApplyColour()
    {
        if (PowerupColourManager.Instance == null)
            return;

        Color chosen =
            PowerupColourManager.Instance
                .GetColour(powerUp);

        foreach (Renderer r in colouredRenderers)
        {
            if (r != null)
            {
                r.material.color = chosen;
            }
        }
    }

    public void ApplyToPlayer(PlayerStats player)
    {
        if (player == null) return;

        switch (powerUp)
        {
            case PowerUpType.SpeedBoost:
                player.ApplyStatChange(0, 0, speedChange);
                break;

            case PowerUpType.DamageBoost:
                player.ApplyStatChange(0, damageChange, 0);
                break;

            case PowerUpType.HealthBoost:
                player.ApplyStatChange(healthChange, 0, 0);
                break;

            case PowerUpType.Weakness:
                player.ApplyStatChange(
                    -healthChange,
                    -damageChange,
                    -speedChange
                );
                break;
        }
    }
}