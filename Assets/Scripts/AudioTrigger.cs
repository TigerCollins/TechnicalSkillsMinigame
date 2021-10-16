using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField]
    AudioLibrary audioLibrary;


    public void PlayHoverSFX()
    {
        AudioPlayer.instance.PlaySoundEffect(audioLibrary.buttonHover);
    }

    public void PlayPressSFX()
    {
        AudioPlayer.instance.PlaySoundEffect(audioLibrary.buttonClick);
    }

    //This is called by FEELS
    public void PlayTimeEndSFX()
    {
        AudioPlayer.instance.PlaySoundEffect(audioLibrary.timeEnd);
    }

}
