using TMPro;
using UnityEngine;

public class CanvasView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    public void UpdateTimeToDisplay(int seconds)
    {
        timerText.text = $"{seconds:D3}";
    }
}
