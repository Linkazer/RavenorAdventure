using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : RVN_Singleton<AudioManager>
{
    [SerializeField] private AudioMixer mixer;

    [SerializeField] private Vector2 soundLimits;

    [SerializeField] private List<string> soundParameters;

    private void Start()
    {
        foreach (string param in soundParameters)
        {
            if (PlayerPrefs.HasKey(param))
            {
                SetMixerSound(param, PlayerPrefs.GetFloat(param));
            }
        }
    }

    /// <summary>
    /// Met à jour le niveau sonore d'un mixer.
    /// </summary>
    /// <param name="exposedParameter">The exposed parameter of the mixer.</param>
    /// <param name="valuePercent">The value between 0 and 1.</param>
    public static void SetMixerSound(string exposedParameter, float valuePercent)
    {
        if (valuePercent == 0)
        {
            instance.mixer.SetFloat(exposedParameter, -80f);
        }
        else
        {
            instance.mixer.SetFloat(exposedParameter, CalculateSound(valuePercent));
        }
    }
    private static float CalculateSound(float percent)
    {
        return percent * (instance.soundLimits.y - instance.soundLimits.x) + instance.soundLimits.x;
    }
}
