using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private PlayerDataSaver playerDataSaver;
    
    public void RestartMinefield()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevelByIndex(int index)
    {
        if (playerDataSaver.NicknameAvailabilityCheck())
        {
            SceneManager.LoadScene(index);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
