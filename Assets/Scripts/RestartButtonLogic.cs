using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButtonLogic : MonoBehaviour
{
    public void RestartMinefield()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
