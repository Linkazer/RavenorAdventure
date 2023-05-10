using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : RVN_Singleton<MusicManager>
{
    [SerializeField] private RVN_AudioPlayer audioPlayer;
    [SerializeField] private AudioData music;

    [SerializeField] private float transitionTime;

    protected override void Awake()
    {
        if(instance != null)
        {
            if (music != null && music != instance.music)
            {
                instance.SetMusic(music);

                instance.AskToPlayMusic(false);
            }
        }
        else if(music != null)
        {
            AskToPlayMusic(false);
        }

        base.Awake();

        
    }

    public void SetMusic(AudioData toSet)
    {
        music = toSet;
    }

    /// <summary>
    /// Set a new music track
    /// </summary>
    /// <param name="forcePlay">If TRUE, there will be no transition between the 2 musics.</param>
    public void AskToPlayMusic(bool forcePlay)
    {
        StopAllCoroutines();
        if (!forcePlay)
        {
            StartCoroutine(SlowStopMusic(PlayMusic));
        }
        else
        {
            audioPlayer.SetVolume(1);
            PlayMusic();
        }
    }

    private void PlayMusic()
    {
        audioPlayer.PlaySound(music);
    }

    public void StopMusic()
    {
        StopAllCoroutines();
        StartCoroutine(SlowStopMusic(null));
    }

    private IEnumerator SlowStopMusic(Action callback)
    {
        float currentTransitionTime = 0;

        while(currentTransitionTime < transitionTime)
        {
            currentTransitionTime += Time.deltaTime;
            audioPlayer.SetVolume(1 - (currentTransitionTime / transitionTime));
            yield return null;
        }

        callback?.Invoke();

        while (currentTransitionTime > 0)
        {
            audioPlayer.SetVolume(1 - (currentTransitionTime / transitionTime));
            currentTransitionTime -= Time.deltaTime;
            yield return null;
        }

        audioPlayer.SetVolume(1);
    }
}
