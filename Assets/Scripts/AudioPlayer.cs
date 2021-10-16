using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    internal static AudioPlayer instance;
    [SerializeField]
    AudioLibrary audioLibrary;

    [Space(10)]

    [SerializeField]
    internal AudioSource soundtrackSource;
    [SerializeField]
    internal AudioSource sfxSource;

    private void Awake()
    {
        //Sets default volume to 3 if the games never been played before, otherwise loads last volume set
        if(FirstTimePlaying)
        {
            audioLibrary.MasterVolume = 3;
        }

        else
        {
            audioLibrary.MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
        }

        //Keeps audio object active inbetwen scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        //Destroys duplicate audio players when changing scenes
        else
        {
            Destroy(this.gameObject);
        }

        //Begins game soundtrack
        PlaySoundtrack(audioLibrary.mainSoundtrack);

        //In this instance, if the game scene is on the initiator, destroy
        if(SceneManager.GetActiveScene().buildIndex ==0)
        {
            SceneManager.LoadSceneAsync(1);
        }
    }

    public void PlaySoundtrack(AudioClip track)
    {
        soundtrackSource.volume = audioLibrary.MusicVolume;
        soundtrackSource.clip = track;
        soundtrackSource.Play();
    }

    public void PlaySoundEffect(AudioClip sfx)
    {
        sfxSource.volume = audioLibrary.SFXVolume;
        sfxSource.PlayOneShot(sfx);
    }

    bool FirstTimePlaying
    {
        get
        {
            bool value = false;

            if(PlayerPrefs.GetInt("TimesPlayed")==0)
            {
                value = true;
            }

            return value;
        }
       
    }
   
}
