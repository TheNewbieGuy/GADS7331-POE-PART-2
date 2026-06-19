using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndSequenceUI : MonoBehaviour
{
    public static EndSequenceUI Instance;

    [Header("Next Wave Button")]
    [SerializeField]
    private GameObject nextWaveButton;

    [SerializeField]
    private CanvasGroup nextWaveCanvasGroup;

    [SerializeField]
    private float buttonFadeDuration = 0.5f;

    [Header("Fortune Text UI")]
    [SerializeField]
    private RectTransform fortuneTextTransform;

    [SerializeField]
    private TextMeshProUGUI fortuneText;

    [Header("Start Delay")]
    [SerializeField]
    private float animationStartDelay = 1f;

    [Header("Scale Animation")]
    [SerializeField]
    private Vector3 targetScale = Vector3.one;

    [SerializeField]
    private float scaleDuration = 1f;

    [SerializeField]
    private AnimationCurve scaleCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Text Movement")]
    [SerializeField]
    private float moveUpDistance = 200f;

    [SerializeField]
    private float moveDuration = 2f;

    [SerializeField]
    private AnimationCurve moveCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Image Slide In")]
    [SerializeField]
    private RectTransform slideImage;

    [SerializeField]
    private float imageStartX = 2000f;

    [SerializeField]
    private float imageMoveDuration = 1.5f;

    [SerializeField]
    private AnimationCurve imageCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector2 textStartPosition;

    private Vector2 imageTargetPosition;

    private void Awake()
    {
        Instance = this;
    }

    public void HideEndUI()
    {
        fortuneTextTransform.localScale =
            Vector3.zero;

        if (nextWaveButton != null)
        {
            nextWaveButton.SetActive(false);
        }

        if (nextWaveCanvasGroup != null)
        {
            nextWaveCanvasGroup.alpha = 0f;
        }
    }

    private void Start()
    {
        if (nextWaveButton != null)
        {
            nextWaveButton.SetActive(false);
        }

        if (nextWaveCanvasGroup != null)
        {
            nextWaveCanvasGroup.alpha = 0f;
        }

        textStartPosition =
            fortuneTextTransform.anchoredPosition;

        fortuneTextTransform.localScale =
            Vector3.zero;

        if (slideImage != null)
        {
            imageTargetPosition =
                slideImage.anchoredPosition;

            slideImage.anchoredPosition =
                new Vector2(
                    imageStartX,
                    imageTargetPosition.y
                );
        }
    }

    public void PlayEndSequence(string fortune)
    {
        fortuneText.text = fortune;

        StopAllCoroutines();

        StartCoroutine(AnimateSequence());
    }

    private IEnumerator AnimateSequence()
    {
        if (slideImage != null)
        {
            StartCoroutine(AnimateImage());
        }

        yield return new WaitForSecondsRealtime(
            animationStartDelay
        );

        yield return AnimateFortune();
    }

    private IEnumerator AnimateImage()
    {
        Vector2 startPosition =
            new Vector2(
                imageStartX,
                imageTargetPosition.y
            );

        float timer = 0f;

        while (timer < imageMoveDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t =
                Mathf.Clamp01(
                    timer / imageMoveDuration
                );

            float curveValue =
                imageCurve.Evaluate(t);

            slideImage.anchoredPosition =
                Vector2.LerpUnclamped(
                    startPosition,
                    imageTargetPosition,
                    curveValue
                );

            yield return null;
        }

        slideImage.anchoredPosition =
            imageTargetPosition;
    }

    private IEnumerator AnimateFortune()
    {
        fortuneTextTransform.localScale =
            Vector3.zero;

        fortuneTextTransform.anchoredPosition =
            textStartPosition;

        float timer = 0f;

        while (timer <
               Mathf.Max(
                   scaleDuration,
                   moveDuration))
        {
            timer += Time.unscaledDeltaTime;

            float scaleT =
                Mathf.Clamp01(
                    timer / scaleDuration);

            float scaleEval =
                scaleCurve.Evaluate(scaleT);

            fortuneTextTransform.localScale =
                Vector3.LerpUnclamped(
                    Vector3.zero,
                    targetScale,
                    scaleEval
                );

            
            float moveT =
                Mathf.Clamp01(
                    timer / moveDuration);

            float moveEval =
                moveCurve.Evaluate(moveT);

            Vector2 targetPosition =
                textStartPosition +
                Vector2.up * moveUpDistance;

            fortuneTextTransform.anchoredPosition =
                Vector2.LerpUnclamped(
                    textStartPosition,
                    targetPosition,
                    moveEval
                );

            yield return null;
        }

        if (nextWaveButton != null)
        {
            nextWaveButton.SetActive(true); 
        }

        if (nextWaveCanvasGroup != null)
        {
            nextWaveCanvasGroup.alpha = 0f; 
            yield return StartCoroutine(FadeInButton());
        }
    }

    private IEnumerator FadeInButton()
    {
        nextWaveCanvasGroup.alpha = 0f;

        float timer = 0f;

        while (timer < buttonFadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t =
                Mathf.Clamp01(
                    timer / buttonFadeDuration
                );

            nextWaveCanvasGroup.alpha = t;

            yield return null;
        }

        nextWaveCanvasGroup.alpha = 1f;
    }
}