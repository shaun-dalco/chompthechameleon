using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    // Assign your sounds in the Inspector
    public AudioClip chompSound;
    public AudioClip tongueSound;
    public AudioClip victorySound;
    public AudioClip errorSound;

    private Dictionary<string, AudioClip> soundMap;

    void Awake()
    {
        soundMap = new Dictionary<string, AudioClip>
        {
            { "chomp", chompSound },
            { "tongue", tongueSound },
            { "win", victorySound },
            { "error", errorSound }
        };
    }

    public void PlaySound(string soundKey)
    {
        if (soundMap.TryGetValue(soundKey, out AudioClip clip))
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound key not found: " + soundKey);
        }
    }
}
