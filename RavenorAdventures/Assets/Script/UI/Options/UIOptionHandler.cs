using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIOptionHandler : MonoBehaviour
{
    [Serializable]
    private class SoundSlider
    {
        public string parameter;
        public Slider slider;
    }

    [SerializeField] private List<SoundSlider> soundSliders;

    public void UE_SetSound(int sliderIndex)
    {
        AudioManager.SetMixerSound(soundSliders[sliderIndex].parameter, soundSliders[sliderIndex].slider.value);
    }
}
