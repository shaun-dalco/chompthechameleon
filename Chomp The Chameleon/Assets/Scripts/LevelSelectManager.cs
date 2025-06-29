using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public Button[] levelButtons; // Assign 15 buttons in order in Inspector
    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.white;

    private const string SaveKey = "LevelProgress";
    private int levelsUnlocked = 1;

    void Start()
    {
        // Load saved progress (default to 1 if no key exists)
        levelsUnlocked = PlayerPrefs.GetInt(SaveKey, 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;

            if (levelIndex <= levelsUnlocked)
            {
                levelButtons[i].interactable = true;
                levelButtons[i].GetComponent<Image>().color = unlockedColor;
                int capturedIndex = i; // Capture for closure
                levelButtons[i].onClick.AddListener(() => LoadLevel(capturedIndex));
            }
            else
            {
                levelButtons[i].interactable = false;
                levelButtons[i].GetComponent<Image>().color = lockedColor;
            }
        }
    }

    public void LoadLevel(int index)
    {
        string sceneName = "Level" + (index + 1); // e.g., "Level1", "Level2", etc.
        SceneManager.LoadScene(sceneName);
    }

    public static void MarkLevelComplete(int level)
    {
        int current = PlayerPrefs.GetInt(SaveKey, 1);
        if (level >= current)
        {
            PlayerPrefs.SetInt(SaveKey, level + 1); // Unlock next level
            PlayerPrefs.Save();
        }
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(SaveKey);
    }
}
