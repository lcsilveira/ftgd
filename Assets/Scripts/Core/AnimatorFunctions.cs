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

[Serializable]
public class ParticleBank
{
    public string name;
    public ParticleSystem particleSystem;
    public int emitAmount;
}

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] SoundBank[] soundBank;
    [SerializeField] ParticleBank[] particleBank;

    public void PlaySound(string soundName)
    {
        SoundBank searchResult = soundBank.FirstOrDefault(s => s.name == soundName);
        if (searchResult != null)
        {
            AudioClip clip = searchResult.variations[UnityEngine.Random.Range(0, searchResult.variations.Length)];
            NewPlayer.Instance.sfxAudioSource.PlayOneShot(clip, searchResult.volume);
        }
    }

    public void EmitParticles(string particleName)
    {
        ParticleBank searchResult = particleBank.FirstOrDefault(p => p.name == particleName);
        if (searchResult != null)
        {
            searchResult.particleSystem.Emit(searchResult.emitAmount);
        }
    }
}
