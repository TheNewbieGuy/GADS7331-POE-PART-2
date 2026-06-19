using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats stats;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 15f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (stats == null)
            stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float speed = stats != null ? stats.GetMoveSpeed() : 6f;

        Vector3 movement =
            moveDirection * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);
    }

    private void Rotate()
    {
        if (moveDirection == Vector3.zero)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(moveDirection);

        Quaternion smoothRotation =
            Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );

        rb.MoveRotation(smoothRotation);
    }
}