using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    public static WaveUI Instance;

    [SerializeField] private TextMeshProUGUI waveText;

    private int total;
    private int remaining;

    private void Awake()
    {
        Instance = this;
    }

    public void SetTotal(int value)
    {
        total = value;
        UpdateText();
    }

    public void SetRemaining(int value)
    {
        remaining = value;
        UpdateText();
    }

    private void UpdateText()
    {
        waveText.text =
            "Enemies Remaining: " +
            remaining +
            " / " +
            total;
    }
}