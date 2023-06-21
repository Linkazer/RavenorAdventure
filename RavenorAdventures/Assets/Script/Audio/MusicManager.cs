using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : RVN_Singleton<MusicManager>
{
    [SerializeField] private RVN_AudioPlayer audioPlayer;
    [SerializeField] private AudioData music;

    [SerializeField] private float transitionTime;

    private AudioData substituteMusic;

    public AudioData CurrentMusic => music;

    protected override void Awake()
    {
        if(instance != null)
        {
            if (music != null && music != instance.music)
            {
                instance.SetMainMusic(music);

                instance.AskToPlayMusic(music, false);;
            }
        }
        else if(music != null)
        {
            AskToPlayMusic(music, false);
        }

        base.Awake();

        
    }

    private void SetMainMusic(AudioData toSet)
    {
        music = toSet;
    }

    public void PlayMainMusic()
    {
        substituteMusic = null;

        AskToPlayMusic(music, false);
    }

    public void PlayMainMusic(AudioData toSet)
    {
        music = toSet;

        PlayMainMusic();
    }

    public void PlaySubsituteMusic(AudioData toPlay, bool forcePlay)
    {
        if (substituteMusic != toPlay)
        {
            substituteMusic = toPlay;

            AskToPlayMusic(toPlay, forcePlay);
        }
    }

    /// <summary>
    /// Set a new music track
    /// </summary>
    /// <param name="forcePlay">If TRUE, there will be no transition between the 2 musics.</param>
    public void AskToPlayMusic(AudioData toPlay, bool forcePlay)
    {
        StopAllCoroutines();
        if (!forcePlay)
        {
            StartCoroutine(SlowStopMusic(() => PlayMusic(toPlay)));
        }
        else
        {
            audioPlayer.SetVolume(1);
            PlayMusic(toPlay);
        }
    }

    private void PlayMusic(AudioData toPlay)
    {
        audioPlayer.PlaySound(toPlay);
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
