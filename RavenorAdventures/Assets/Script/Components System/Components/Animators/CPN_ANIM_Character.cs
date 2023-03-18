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

    [SerializeField] private CharacterAnimation jumpOnTargetAnimation;
    [SerializeField] private CharacterAnimation idleSpriteAnim;
    private CharacterAnimationType currentAnimation = CharacterAnimationType.None;

    private CharacterAnimationData currentAnim;

    public override void OnEnterBattle()
    {

    }

    public override void OnExitBattle()
    {
        
    }

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

   /*public void SetWalkAnimation(bool value)
    {
        if(value)
        {
            SetAnimation(CharacterAnimationType.Walk);
        }
        else
        {
            SetAnimation(CharacterAnimationType.Idle);
        }
    }

    public void SetDeathAnimation(bool value)
    {
        if(value)
        {
            SetAnimation(CharacterAnimationType.Death);
        }
        else
        {
            SetAnimation(CharacterAnimationType.Idle);
        }
    }

    public void SetCastSpellAnimation(LaunchedSpellData launchedSpell)
    {
        switch(launchedSpell.scriptable.CastingAnimation)
        {
            case CharacterAnimationType.JumpOnTarget:
                //jumpOnTargetAnimation.Play((Vector2)launchedSpell.targetNode.worldPosition);
                break;
            default:
                SetAnimation(launchedSpell.scriptable.CastingAnimation);
                TimerManager.CreateGameTimer(launchedSpell.scriptable.CastDuration, () => SetAnimation(CharacterAnimationType.LaunchSpell));
                break;
        }
    }*/

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

    public void EndAnimation()
    {
        PlayAnimation("Idle");
        currentAnim = null;
    }

    /*protected void SetAnimation(CharacterAnimationType toSet)
    {
        Debug.Log($"Set Anim : {toSet} != {currentAnimation}");
        if (toSet != currentAnimation)
        {
            UnsetAnimation(currentAnimation);

            currentAnimation = toSet;
            switch(toSet)
            {
                case CharacterAnimationType.Idle:
                    AnimSetBool("IsIdle", true);
                    //idleSpriteAnim.Play(Vector2.zero);
                    break;
                case CharacterAnimationType.Walk:
                    AnimSetBool("IsWalking", true);
                    break;
                case CharacterAnimationType.CastSpell:
                    AnimSetBool("IsCasting", true);
                    break;
                case CharacterAnimationType.LaunchSpell:
                    animator.SetTrigger("IsSpellLaunch");
                    break;
                case CharacterAnimationType.Death:
                    Debug.Log("Set Death");
                    AnimSetBool("IsDead", true);
                    break;
                case CharacterAnimationType.Shoot:
                    AnimSetTrigger("Shoot");
                    break;
                case CharacterAnimationType.Throw:
                    AnimSetTrigger("Throw");
                    break;
            }
        }
    }

    protected void UnsetAnimation(CharacterAnimationType toUnset)
    {
        if(toUnset == currentAnimation)
        {
            currentAnimation = CharacterAnimationType.Idle;
            switch (toUnset)
            {
                case CharacterAnimationType.Idle:
                    AnimSetBool("IsIdle", false);
                    idleSpriteAnim.Stop();
                    break;
                case CharacterAnimationType.Walk:
                    AnimSetBool("IsWalking", false);
                    break;
                case CharacterAnimationType.CastSpell:
                    AnimSetBool("IsCasting", false);
                    break;
                case CharacterAnimationType.LaunchSpell:
                    AnimSetBool("IsSpellLaunch", false);
                    break;
                case CharacterAnimationType.Death:
                    AnimSetBool("IsDead", false);
                    break;
            }

            if(toUnset != CharacterAnimationType.Idle)
            {
                SetAnimation(currentAnimation);
            }
        }
    }*/
}
