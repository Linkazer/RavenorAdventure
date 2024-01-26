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

    public override void SetComponent(RVN_ComponentHandler handler)
    {
        base.SetComponent(handler);

        CharacterScriptable_Battle character = (handler as CPN_Character).Scriptable;

        characterSprite.sprite = character.GameSprite();

        if (character.HandSprite != null)
        {
            handHolder.localPosition = new Vector3(0, 0.2f + character.HandHeight, 0);

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

    public override void Activate()
    {
        base.Activate();

        PlayAnimation("Idle");
    }

    public void SetSprite()
    {
        Debug.Log("Should not pass here");
    }

    public void SetCharacter(CharacterScriptable_Battle character)
    {
        Debug.Log("Should not pass here");
    }

    public void SetOrientation(Vector2 direction) //Appelé par des Unity Events (voir si besoin de changer)
    {
        if(direction.x > 0)
        {
            rendererTransform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if(direction.x < 0)
        {
            rendererTransform.localEulerAngles = new Vector3(0, -180, 0);
        }

        animator.SetFloat("OrientationX", direction.x);
        animator.SetFloat("OrientationY", direction.y);
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
