using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("AudioSources")]
    public AudioSource bgAudioSource;
    public AudioSource mainAudioSource;

    [Header("AudioClips")]
    public AudioClip glassBreak;
    public AudioClip obstacleHit;
    public AudioClip poopHit;
    public AudioClip slideWoosh1;
    public AudioClip slideWoosh;
    public AudioClip chargingStarted;
    public AudioClip maleAngry;
    public AudioClip win;
    public AudioClip fail;
    public AudioClip girlHit;
    public AudioClip slide;
    public AudioClip charging;
    public AudioClip confettiBlast;
    public AudioClip coin;
    public List<AudioClip> malePainReactions;
    private void Awake()
    {
        instance = this;
    }

    public void PlayClip(AudioClip clip)
    {
        mainAudioSource.PlayOneShot(clip);
    }

    public IEnumerator PlayMaleHitSound()
    {
        int index = Random.Range(0, malePainReactions.Count);
        mainAudioSource.PlayOneShot(malePainReactions[index]);

        yield return new WaitForSeconds(1);
        mainAudioSource.PlayOneShot(maleAngry);
    }
}
