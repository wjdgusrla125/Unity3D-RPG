using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider BGMSlider;
    public Slider SFXSlider;

    public void BGMVolumeControl()
    {
        float BGMsound = BGMSlider.value;

        if (BGMsound == -40f)
        {
            audioMixer.SetFloat("BGMParam", -80);
        }
        else
        {
            audioMixer.SetFloat("BGMParam", BGMsound);
        }
    }
    
    public void SFXVolumeControl()
    {
        float SFXsound = SFXSlider.value;

        if (SFXsound == -40f)
        {
            audioMixer.SetFloat("SFXParam", -80);
        }
        else
        {
            audioMixer.SetFloat("SFXParam", SFXsound);
        }
    }
}
