using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected int baseHealth = 5;
    [SerializeField] protected int baseDamage = 1;
    [SerializeField] protected float baseMoveSpeed = 3f;

    [Header("Current Stats")]
    [SerializeField] protected int health;
    [SerializeField] protected int damage;
    [SerializeField] protected float moveSpeed;

    public System.Action OnDeath;

    [HideInInspector] public string fortune;

    protected GameObject player;

    protected virtual void Awake()
    {
        ResetToBaseStats();
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        EnemyManager.Instance?.Register(this);
    }

    public void ResetToBaseStats()
    {
        health = baseHealth;
        damage = baseDamage;
        moveSpeed = baseMoveSpeed;
    }

   
    public void SetHealth(int value)
    {
        health = value;
    }

    public void SetDamage(int value)
    {
        damage = value;
    }

    public void SetMoveSpeed(float value)
    {
        moveSpeed = value;
    }
    public int GetHealth() => health;

    public float GetMoveSpeed() => moveSpeed;

    public int GetDamage() => damage;
    
    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    protected virtual void Die()
    {
        EnemyManager.Instance?.HandleEnemyDeath(this);
        OnDeath?.Invoke();
    }
}