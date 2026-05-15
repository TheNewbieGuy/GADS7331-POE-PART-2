using System.Collections;
using UnityEngine;

public class FortuneCookieHalf : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private Vector3 moveDirection = Vector3.up;

    [SerializeField]
    private float moveDistance = 100f;

    [SerializeField]
    private float moveDuration = 1f;

    [SerializeField]
    private float startDelay = 0f;

    [SerializeField]
    private AnimationCurve moveCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.localPosition;
    }

    public void OpenCookie()
    {
        StopAllCoroutines();

        StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        yield return new WaitForSecondsRealtime(
            startDelay
        );

        Vector3 targetPosition =
            startPosition +
            moveDirection.normalized *
            moveDistance;

        float timer = 0f;

        while (timer < moveDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t =
                Mathf.Clamp01(timer / moveDuration);

            float curveValue =
                moveCurve.Evaluate(t);

            transform.localPosition =
                Vector3.LerpUnclamped(
                    startPosition,
                    targetPosition,
                    curveValue
                );

            yield return null;
        }

        transform.localPosition =
            targetPosition;
    }
}