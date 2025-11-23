using UnityEngine;

public class PlayerVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject rightPlayerHandle;
    [SerializeField] private GameObject leftPlayerHandle;
    [SerializeField] private GameObject upPlayerHandle;
    [SerializeField] private GameObject downPlayerHandle;

    public void SetActiveRightPlayerHandle(bool active)
    {
        rightPlayerHandle.SetActive(active);
    }

    public void SetActiveLeftPlayerHandle(bool active)
    {
        leftPlayerHandle.SetActive(active);
    }

    public void SetActiveUpPlayerHandle(bool active)
    {
        upPlayerHandle.SetActive(active);
    }

    public void SetActiveDownPlayerHandle(bool active)
    {
        downPlayerHandle.SetActive(active);
    }
}
