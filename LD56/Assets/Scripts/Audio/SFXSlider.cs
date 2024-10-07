using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXSlider : MonoBehaviour
{
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = AudioManager.instance.sfxVolume;

        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }


    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        AudioManager.instance.SFXVolume(slider.value);
    }
}
