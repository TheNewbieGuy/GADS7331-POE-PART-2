using System.Collections.Generic;
using UnityEngine;

public class PowerupColourManager : MonoBehaviour
{
    public static PowerupColourManager Instance;

    [SerializeField]
    private Color[] availableColours;

    private Dictionary<EnemyPowerup.PowerUpType, Color>
        colourMap =
            new Dictionary<EnemyPowerup.PowerUpType, Color>();

    private void Awake()
    {
        Instance = this;

        GenerateColourMapping();
    }

    public void GenerateColourMapping()
    {
        List<Color> colours =
            new List<Color>(availableColours);

        Shuffle(colours);

        colourMap[
            EnemyPowerup.PowerUpType.SpeedBoost
        ] = colours[0];

        colourMap[
            EnemyPowerup.PowerUpType.DamageBoost
        ] = colours[1];

        colourMap[
            EnemyPowerup.PowerUpType.HealthBoost
        ] = colours[2];

        colourMap[
            EnemyPowerup.PowerUpType.Weakness
        ] = colours[3];

        Debug.Log("New colour mapping generated.");
    }

    public Color GetColour(
        EnemyPowerup.PowerUpType type)
    {
        return colourMap[type];
    }

    private void Shuffle(List<Color> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex =
                Random.Range(i, list.Count);

            (list[i], list[randomIndex]) =
                (list[randomIndex], list[i]);
        }
    }
}