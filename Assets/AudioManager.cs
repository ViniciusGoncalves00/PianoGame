using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;

    private ObjectPool<AudioSource> _audioSourcePool;

    private void Awake()
    {
        _audioSourcePool = new ObjectPool<AudioSource>(() => gameObject.AddComponent<AudioSource>());
    }

    public void PlaySound(int index)
    {
        StartCoroutine(PlaySoundCoroutine(index));
    }

    private IEnumerator PlaySoundCoroutine(int index)
    {
        var audioClip = _audioClips[index];
        using var rentedObject = _audioSourcePool.Get(out var audioSource);
        
        audioSource.PlayOneShot(audioClip);
        
        yield return new WaitForSeconds(audioClip.length);
    }
}