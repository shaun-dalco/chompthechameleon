using UnityEngine;

public class LevelMonitor : MonoBehaviour
{
    public GameObject victoryPopup;
    public GameObject failurePopup;
    public Transform chameleonHead;
    public float fallThresholdY = -10f;

    public ParticleSystem[] victoryParticles; // assign both in the Inspector

    private bool levelEnded = false;

    private const string SaveKey = "LevelProgress";

    public int thisLevel;

    void Update()
    {
        if (levelEnded) return;

        CheckForVictory();
        CheckForFailure();
    }

    void CheckForVictory()
    {
        GameObject[] peaches = GameObject.FindGameObjectsWithTag("Peach");
        GameObject[] watermelons = GameObject.FindGameObjectsWithTag("Watermelon");

        if (peaches.Length == 0 && watermelons.Length == 0)
        {
            levelEnded = true;

            int currentMaxLevel = PlayerPrefs.GetInt(SaveKey, 1);
            if(thisLevel == currentMaxLevel) {
                PlayerPrefs.SetInt(SaveKey, thisLevel+1);
            }

            if (victoryPopup != null)
                victoryPopup.SetActive(true);

            foreach (var ps in victoryParticles)
            {
                if (ps != null)
                    ps.gameObject.SetActive(true);
                    ps.Play();
            }
        }
    }

    void CheckForFailure()
    {
        if (chameleonHead.position.y < fallThresholdY)
        {
            levelEnded = true;

            if (failurePopup != null)
                failurePopup.SetActive(true);
        }
    }
}
