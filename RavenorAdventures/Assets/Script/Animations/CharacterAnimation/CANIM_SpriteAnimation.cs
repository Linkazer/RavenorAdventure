using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CANIM_SpriteAnimation : CharacterAnimation
{
    [SerializeField] private SpriteRenderer rnd;

    [SerializeField] private Sprite[] animation;

    [SerializeField] private float animationFps;

    private int currentFrameIndex;
    private float currentFrameTime;

    private float frameTime => 1 / animationFps;

    private void Update()
    {
        currentFrameTime += Time.deltaTime;

        if(currentFrameTime >= frameTime)
        {
            currentFrameTime -= frameTime;

            currentFrameIndex = (currentFrameIndex + 1) % animation.Length;

            rnd.sprite = animation[currentFrameIndex];
        }
    }

    public override void Play(Vector2 _targetPosition)
    {
        Debug.Log("Play Here");

        currentFrameTime = 0;
        currentFrameIndex = 0;

        rnd.sprite = animation[currentFrameIndex];

        if (animation.Length > 0)
        {
            enabled = true;
        }
    }

    public override void Stop()
    {
        enabled = false;
    }

    public override void SetCharacter(CharacterScriptable_Battle character)
    {
        animation = new Sprite[character.characterIdleAnimation.Count];

        int i = 0;

        foreach(Sprite spr in character.characterIdleAnimation)
        {
            animation[i] = spr;
            i++;
        }
    }
}
