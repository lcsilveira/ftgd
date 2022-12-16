using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class SoundBank
{
    public string name;
    public AudioClip[] variations;
    [Range(0f, 1f)]
    public float volume = 1f;
}

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] SoundBank[] soundBank;

    public void PlaySound(string soundName)
    {
        SoundBank searchResult = soundBank.FirstOrDefault(s => s.name == soundName);
        if (searchResult != null) {
            AudioClip clip = searchResult.variations[UnityEngine.Random.Range(0, searchResult.variations.Length)];
            NewPlayer.Instance.sfxAudioSource.PlayOneShot(clip, searchResult.volume);
        }
    }
}
