using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected int health = 5;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float moveSpeed = 3f;

    [HideInInspector]
    public string fortune;

    protected GameObject player;

    protected virtual void Start()
    {
        player =
            GameObject.FindGameObjectWithTag("Player");

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.Register(this);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance
                .HandleEnemyDeath(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}