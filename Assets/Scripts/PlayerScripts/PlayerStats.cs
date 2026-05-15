using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int health = 100;

    [SerializeField] private int maxHealth = 100;

    [Header("Damage")]
    [SerializeField] private int damage = 75;

    [Header("Weapon")]
    [SerializeField] private GameObject PlayerWeapon;

    [Header("Death UI")]
    [SerializeField] private GameObject deathUI;

    private bool isDead;

    void Start()
    {
        if (PlayerWeapon != null)
        {
            PlayerWeapon.SetActive(false);
        }

        if (deathUI != null)
        {
            deathUI.SetActive(false);
        }
    }

    public void takedamage(int damageAmount)
    {
        if (isDead)
            return;

        health -= damageAmount;

        // prevent negative health
        if (health < 0)
        {
            health = 0;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        Debug.Log("Player has died.");

        Time.timeScale = 0f;

        if (deathUI != null)
        {
            deathUI.SetActive(true);
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void ResetStatsForNewWave()
    {
        health = maxHealth;

        isDead = false;

        if (deathUI != null)
        {
            deathUI.SetActive(false);
        }
    }

    public void Attack()
    {
        if (PlayerWeapon != null)
        {
            PlayerWeapon.SetActive(true);

            Debug.Log(
                "Player attacks with " +
                PlayerWeapon.name
            );
        }
    }

    public void not_attacking()
    {
        if (PlayerWeapon != null)
        {
            PlayerWeapon.SetActive(false);

            Debug.Log(
                "Player is not attacking."
            );
        }
    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetDamage()
    {
        return damage;
    }
}