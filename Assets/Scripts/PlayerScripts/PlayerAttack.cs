using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 4f;
    [SerializeField] private float attackCooldown = 0.5f;

    [Header("Layers")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Weapon Swing")]
    [SerializeField] private GameObject weaponSwingPrefab;

    private float cooldownTimer;

    private PlayerStats stats;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (cooldownTimer > 0f)
            return;

        cooldownTimer = attackCooldown;

        SpawnWeaponSwing();

        int damage = stats != null ? stats.GetDamage() : 2;

        Collider[] enemies =
            Physics.OverlapSphere(
                transform.position,
                attackRange,
                enemyLayer
            );

        foreach (Collider enemy in enemies)
        {
            EnemyBase enemyBase =
                enemy.GetComponent<EnemyBase>();

            if (enemyBase != null)
            {
                enemyBase.TakeDamage(damage);
            }
        }
    }

    private void SpawnWeaponSwing()
    {
        if (weaponSwingPrefab == null)
            return;

        GameObject swing =
            Instantiate(
                weaponSwingPrefab,
                transform.position,
                Quaternion.identity
            );

        swing.transform.SetParent(transform);
    }
}