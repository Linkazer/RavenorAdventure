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

    [Header("Menus")]
    [SerializeField] private GameObject optionMenu;

    [Header("Options - Sound")]
    [SerializeField] private List<SoundSlider> soundSliders;

    public void UE_OpenOptionMenu(bool state)
    {
        optionMenu.gameObject.SetActive(state);

        foreach(SoundSlider soundSlider in soundSliders)
        {
            if (PlayerPrefs.HasKey(soundSlider.parameter))
            {
                soundSlider.slider.value = PlayerPrefs.GetFloat(soundSlider.parameter);
            }
        }
    }

    public void UE_SetSound(int sliderIndex)
    {
        string parameter = soundSliders[sliderIndex].parameter;
        float value = soundSliders[sliderIndex].slider.value;

        PlayerPrefs.SetFloat(parameter, value);

        AudioManager.SetMixerSound(parameter, value);
    }
}
