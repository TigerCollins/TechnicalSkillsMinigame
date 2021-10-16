using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Audio Library", order = 1)]
public class AudioLibrary : ScriptableObject
{
    /// <summary>
    /// The AudioLibrary object is setup to minimise singletons and serve as a injection dependence. With prior planning, scriptable objects would have been utitlised to
    /// alleviate singletons and monobehaviour overhead
    /// </summary>

    [Header("Functionality")]
    [SerializeField]
    AudioMixer audioMixer;
    [Range(0,10)]
    [SerializeField]
    float masterVolume = 5;
    [Range(0, 10)]
    [SerializeField]
    int sfxVolume = 6;
    [Range(0, 10)]
    [SerializeField]
    int musicVolume = 3;

    [Header("Soundtracks")]
    public AudioClip mainSoundtrack;

    [Header("Sound Effects")]
    public AudioClip buttonHover;
    public AudioClip buttonClick;

    [Space(5)]

    public AudioClip itemPickUp;
    public AudioClip itemDrop;
    public AudioClip itemPacked;

    [Space(5)]

    public AudioClip scoreChange;
    public AudioClip timeChange;

    [Space(5)]

    public AudioClip timeEnd;

    public float MasterVolume
    {
        get
        {
            return masterVolume;
        }

        set
        {
            masterVolume = Mathf.Clamp(value, 0, 10);

            //Sets and saves volume between scenes and lasts through the application being closed/opened
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        }
    }

    public int SFXVolume
    {
        get
        {
            return sfxVolume;
        }
    }

    public int MusicVolume
    {
        get
        {
            return musicVolume;
        }
    }

    public AudioMixer ActiveAudioMixer
    {
        get
        {
            return audioMixer;
        }
    }
}

