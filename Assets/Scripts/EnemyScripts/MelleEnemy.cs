using System.Collections;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    [Header("Melee Settings")]
    [SerializeField] private float attackRange = 2f;

    [SerializeField] private float attackCooldown = 1f;

    [Header("Weapon Swing")]
    [SerializeField] private GameObject weaponSwingPrefab;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 8f;

    [SerializeField] private float knockbackDuration = 0.2f;

    private float cooldownTimer;

    private Rigidbody rb;

    private bool isKnockedBack;

    private GameObject currentSwing;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();

        cooldownTimer = 0f;

        StartCoroutine(GenerateFortune());
    }

    private IEnumerator GenerateFortune()
    {
        while (OllamaManager.Instance == null)
        {
            yield return null;
        }

        yield return OllamaManager.Instance.GenerateFortune(
            (result) =>
            {
                fortune = result;
            }
        );
    }

    private void Update()
    {
        if (player == null)
            return;

        if (isKnockedBack)
            return;

        cooldownTimer -= Time.deltaTime;

        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        Vector3 direction =
            (player.transform.position - transform.position)
            .normalized;

        direction.y = 0f;

        transform.rotation =
            Quaternion.LookRotation(direction);

        float distance =
            Vector3.Distance(
                transform.position,
                player.transform.position
            );

        if (distance > attackRange)
        {
            rb.linearVelocity =
                direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;

            Attack();
        }
    }

    private void Attack()
    {
        if (cooldownTimer > 0f)
            return;

        cooldownTimer = attackCooldown;

        SpawnWeaponSwing();

        PlayerStats stats =
            player.GetComponent<PlayerStats>();

        if (stats != null)
        {
            stats.takedamage(damage);
        }
    }

    private void SpawnWeaponSwing()
    {
        if (weaponSwingPrefab == null)
            return;

        if (currentSwing != null)
        {
            Destroy(currentSwing);
        }

        currentSwing =
            Instantiate(
                weaponSwingPrefab,
                transform.position,
                transform.rotation
            );

        currentSwing.transform.SetParent(transform);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (currentSwing != null)
        {
            Destroy(currentSwing);
        }

        ApplyKnockback();
    }

    private void ApplyKnockback()
    {
        if (player == null)
            return;

        StartCoroutine(KnockbackRoutine());
    }

    private IEnumerator KnockbackRoutine()
    {
        isKnockedBack = true;

        Vector3 direction =
            (transform.position -
             player.transform.position)
            .normalized;

        direction.y = 0f;

        rb.linearVelocity = Vector3.zero;

        rb.AddForce(
            direction * knockbackForce,
            ForceMode.Impulse
        );

        yield return new WaitForSeconds(
            knockbackDuration
        );

        isKnockedBack = false;
    }

    private void OnDestroy()
    {
        if (currentSwing != null)
        {
            Destroy(currentSwing);
        }
    }
}