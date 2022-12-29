using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CANIM_SpriteAnimation : CharacterAnimation
{
    [SerializeField] private SpriteRenderer rnd;

    [SerializeField] private Dictionary<string, Sprite[]> spriteAnimations = new Dictionary<string, Sprite[]>();

    [SerializeField] private float animationFps;

    private int currentFrameIndex;
    private float currentFrameTime;

    private Sprite[] currentSpriteAnimation;

    private float frameTime => 1 / animationFps;

    private void Update()
    {
        currentFrameTime += Time.deltaTime;

        if(currentFrameTime >= frameTime)
        {
            currentFrameTime -= frameTime;

            currentFrameIndex = (currentFrameIndex + 1) % currentSpriteAnimation.Length;

            if(currentFrameIndex == 0 && !currentAnimation.doesLoop)
            {
                End();
            }

            rnd.sprite = currentSpriteAnimation[currentFrameIndex];
        }
    }

    public override void Play(object animationdata)
    {
        currentFrameTime = 0;
        currentFrameIndex = 0;

        currentSpriteAnimation = spriteAnimations[currentAnimation.AnimationName];

        rnd.sprite = currentSpriteAnimation[currentFrameIndex];

        if (currentSpriteAnimation.Length > 0)
        {
            enabled = true;
        }
    }

    public override void Stop()
    {
        enabled = false;
    }

    public override void End()
    {
        Stop();

        if (currentAnimation.linkedAnimation != "")
        {
            currentAnimation.endCallback?.Invoke();
        }
    }

    public override void SetCharacter(CharacterScriptable_Battle character)
    {
        foreach(SpriteAnimation anim in character.animations)
        {
            spriteAnimations.Add(anim.animationName, anim.animation);
        }
    }
}
