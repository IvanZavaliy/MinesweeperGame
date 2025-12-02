using TMPro;
using UnityEngine;

public class CanvasView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    
    [SerializeField] private TextMeshProUGUI flagCounterText;
    
    [SerializeField] private GameObject winMenuPopUp;
    [SerializeField] private GameObject loseMenuPopUp;

    public void UpdateTimeToDisplay(int seconds)
    {
        timerText.text = $"{seconds:D3}";
    }

    public void UpdateFlagCounterToDisplay(int numberOfFlags)
    {
        flagCounterText.text = $"{numberOfFlags:D3}";
    }

    public void ShowWinMenu()
    {
        winMenuPopUp.SetActive(true);
    }

    public void ShowLoseMenu()
    {
        loseMenuPopUp.SetActive(true);
    }
}
