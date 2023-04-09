using ravenor.referencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterAnimationType
{
    None,
    Idle,
    Character_Walk,
    Character_CastSpell,
    Character_LaunchSpell,
    Character_JumpOnTarget,
    Character_Death,
    Character_Shoot,
    Character_Throw,
    Character_Jump,
}

public class CPN_ANIM_Character : CPN_AnimationHandler
{
    [Header("Character Display")]
    [SerializeField] private Transform rendererTransform;
    [SerializeField] private SpriteRenderer characterSprite;
    [SerializeField] private Transform handHolder;
    [SerializeField] private List<SpriteRenderer> handsSprites;

    [Header("Character Animations")]
    [SerializeField] private CharacterAnimationData[] animations;

    [Header("Audio")]
    [SerializeField] private RVN_AudioPlayer audioPlayer;

    private CharacterAnimationData currentAnim;

    public void SetSprite(CharacterScriptable_Battle character)
    {
        characterSprite.sprite = character.GameSprite();

        if (character.HandSprite != null)
        {
            handHolder.localPosition += new Vector3(0, character.HandHeight, 0);

            foreach (SpriteRenderer spr in handsSprites)
            {
                spr.sprite = character.HandSprite;

                if (character.DisplayHand)
                {
                    spr.gameObject.SetActive(true);
                }
            }
        }
    }

    public void SetCharacter(CharacterScriptable_Battle character)
    {
        SetSprite(character);

        PlayAnimation("Idle");
    }

    public void SetOrientation(Vector2 direction)
    {
        if(direction.x > 0)
        {
            rendererTransform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if(direction.x < 0)
        {
            rendererTransform.localEulerAngles = new Vector3(0, -180, 0);
        }
    }

    public void PlayAnimation(string animationName)
    {
        PlayAnimation(animationName, null);
    }

    public void PlayAnimation(string animationName, object animationData)
    {
        if (currentAnim == null || currentAnim.AnimationName != animationName)
        {
            foreach (CharacterAnimationData anim in animations)
            {
                if (animationName == anim.AnimationName)
                {
                    if (currentAnim != null)
                    {
                        currentAnim.Stop();
                    }

                    currentAnim = anim;

                    currentAnim.Play(animationData, () => PlayAnimation(currentAnim.linkedAnimation, animationData));
                }
            }
        }
    }

    public void UE_PlayAnimationSound()
    {
        PlaySound();
    }

    private void PlaySound()
    {
        audioPlayer.PlaySound(currentAnim.GetClip());
    }

    public void EndAnimation()
    {
        PlayAnimation("Idle");
        currentAnim = null;
    }
}
