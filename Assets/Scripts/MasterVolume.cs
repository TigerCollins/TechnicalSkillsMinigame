using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MasterVolume : MonoBehaviour
{
    [SerializeField]
    AudioLibrary audioLibrary;

    [Space(5)]

    [SerializeField]
    Slider masterSlider;
    [SerializeField]
    GameObject sliderFillObject;
    [SerializeField]
    TextMeshProUGUI volumeText;

    public void Start()
    {
        masterSlider.value = audioLibrary.MasterVolume;
    }

    public void ChangeMasterVolume()
    {
        //Adjusts audio on scriptable object
        audioLibrary.MasterVolume = masterSlider.value;

        //Hides audio slider if volume less than 1
        //Changes master volume
        if (audioLibrary.MasterVolume < 1)
        {
            sliderFillObject.SetActive(false);
            audioLibrary.ActiveAudioMixer.SetFloat("MasterVol", -80);
        }

        else
        {
            sliderFillObject.SetActive(true);
            audioLibrary.ActiveAudioMixer.SetFloat("MasterVol", (Mathf.Log10(audioLibrary.MasterVolume) * 20) - 20);

        }

        //Changes volume string
        AdjustVolumeString();

    }

    void AdjustVolumeString()
    {
        volumeText.text = "Volume - " + Mathf.Round(audioLibrary.MasterVolume).ToString();
    }
}
