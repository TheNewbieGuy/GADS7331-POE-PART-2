using UnityEngine;

public class RandomPlayerColor : MonoBehaviour
{
    [Header("Colours")]
    [SerializeField] private Color[] possibleColors;

    [Header("Renderers")]
    [SerializeField] private Renderer[] renderersToColor;

    private void Start()
    {
        ApplyRandomColor();
    }

    private void ApplyRandomColor()
    {
        if (possibleColors == null ||
            possibleColors.Length == 0)
        {
            Debug.LogWarning(
                "No colours assigned."
            );

            return;
        }

        Color chosenColor =
            possibleColors[
                Random.Range(
                    0,
                    possibleColors.Length
                )
            ];

        foreach (Renderer renderer in renderersToColor)
        {
            if (renderer != null)
            {
                renderer.material.color =
                    chosenColor;
            }
        }
    }
}