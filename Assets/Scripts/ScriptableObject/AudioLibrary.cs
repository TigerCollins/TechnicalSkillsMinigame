using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Audio Library", order = 1)]
public class AudioLibrary : ScriptableObject
{
    /// <summary>
    /// The AudioLibrary object is setup to minimise singletons. This is one of the last components made for the technical quiz
    /// and the use of singletons was hire than desired. 
    /// 
    /// To use the audio library correctly, reference the ScriptableObject on the component that references it.
    /// </summary>

    [Header("Functionality")]
    [Range(0,10)]
    [SerializeField]
    int volume;

    [Space(10)]

    [SerializeField]
    AudioSource soundtrackSource;
    [SerializeField]
    AudioSource sfxSource;

    [Header("Soundtracks")]
    public AudioClip mainSoundtrack;

    [Header("Sound Effects")]
    public AudioClip buttonHover;
    public AudioClip buttonClick;

    public void PlaySoundtrack(AudioClip track)
    {
        soundtrackSource.clip = track;
        soundtrackSource.Play();
    }

    public void PlaySoundEffect(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx);
    }

    public int Volume
    {
        get
        {
            return volume;
        }

        set
        {
            volume = Mathf.Clamp(value, 0, 10);
        }
    }
}

