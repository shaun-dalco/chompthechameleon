using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        // Make sure we don't exceed the last scene
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1f; // In case the game was paused
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("No more levels in build index.");
        }
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // In case the game was paused
        SceneManager.LoadScene(0);
    }
}
