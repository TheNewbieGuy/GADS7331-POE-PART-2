using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats (Original Values)")]
    [SerializeField] private int baseHealth = 50;
    [SerializeField] private int baseMaxHealth = 50;
    [SerializeField] private int baseDamage = 1;
    [SerializeField] private float baseMoveSpeed = 5f;

    [Header("Current Stats")]
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed;

    [Header("Limits")]
    [SerializeField] private float minMoveSpeed = 3f;
    [SerializeField] private float maxMoveSpeed = 8f;

    [SerializeField] private int minHealth = 25;
    [SerializeField] private int maxHealthLimit = 150;

    [SerializeField] private float damageCooldown = 0.5f;
    private float lastDamageTime;

    [Header("Death UI")]
    [SerializeField] private GameObject deathUI;

    private bool isDead;

    private void Start()
    {
        ResetToBaseStats();
    }

    public void ResetToBaseStats()
    {
        health = Mathf.Clamp(baseHealth, minHealth, maxHealthLimit);
        maxHealth = Mathf.Clamp(baseMaxHealth, minHealth, maxHealthLimit);

        damage = baseDamage;

        moveSpeed = Mathf.Clamp(baseMoveSpeed, minMoveSpeed, maxMoveSpeed);

        isDead = false;
        Time.timeScale = 1f;

        if (deathUI != null)
            deathUI.SetActive(false);
    }

    public void takedamage(int amount)
    {
        if (isDead) return;

        if (Time.time < lastDamageTime + damageCooldown)
            return;

        lastDamageTime = Time.time;

        health -= amount;
        health = Mathf.Max(0, health);

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        Time.timeScale = 0f;

        if (deathUI != null)
            deathUI.SetActive(true);
    }

    public void ApplyStatChange(int hp, int dmg, float speed)
    {
        maxHealth = Mathf.Clamp(maxHealth + hp, minHealth, maxHealthLimit);
        health = Mathf.Clamp(health + hp, minHealth, maxHealthLimit);

        damage = Mathf.Clamp(damage + dmg, 1, 8);
        moveSpeed = Mathf.Clamp(moveSpeed + speed, minMoveSpeed, maxMoveSpeed);
    }

    public void ResetStatsForNewWave()
    {
        health = Mathf.Clamp(maxHealth, minHealth, maxHealthLimit);
        isDead = false;

        Time.timeScale = 1f;

        if (deathUI != null)
            deathUI.SetActive(false);
    }

    public void DebugPrintStats()
    {
        Debug.Log(
            "PLAYER STATS => " +
            "HP: " + health + "/" + maxHealth +
            " | DMG: " + damage +
            " | SPEED: " + moveSpeed
        );
    }
    public int GetCurrentHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public int GetDamage() => damage;
    public float GetMoveSpeed() => moveSpeed;
}