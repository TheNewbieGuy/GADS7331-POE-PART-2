using UnityEngine;

public class WeaponSwing : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 720f;

    [SerializeField]
    private float lifetime = 0.25f;

    private float rotatedAmount;

    private void Update()
    {
        float rotationThisFrame =
            rotationSpeed * Time.deltaTime;

        transform.Rotate(
            0f,
            rotationThisFrame,
            0f
        );

        rotatedAmount += rotationThisFrame;

        if (rotatedAmount >= 360f)
        {
            Destroy(gameObject);
        }
    }
}