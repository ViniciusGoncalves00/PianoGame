using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private List<AudioSource> audioSources;

    private void Awake()
    {
        AddAudioSource();
    }

    private void AddAudioSource()
    {
        var item = gameObject.AddComponent<AudioSource>();
        audioSources.Add(item);
    }

    public void PlaySound(int index)
    {
        var audioClip = audioClips[index];
            
        foreach (var audioSource in audioSources)
        {
            if (audioSource.isPlaying)
                continue;
            
            PlayAudio(audioSource, audioClip);
            return;
        }
        
        AddAudioSource();
 
        var newAudioSource = audioSources[^1];
        
        PlayAudio(newAudioSource, audioClip);
    }

    private void PlayAudio(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        StartCoroutine(WaitForAudio(audioSource));
    }

    private IEnumerator WaitForAudio(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        audioSource.Stop();
    }
}