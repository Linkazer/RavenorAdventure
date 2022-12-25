using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterAnimationType
{
    None,
    Idle,
    Walk,
    CastSpell,
    LaunchSpell,
    JumpOnTarget,
    Death,
    Shoot,
    Throw,
}

public class CPN_ANIM_Character : CPN_AnimationHandler
{
    [Header("Character Display")]
    [SerializeField] private Transform rendererTransform;
    [SerializeField] private SpriteRenderer characterSprite;
    [SerializeField] private List<SpriteRenderer> handsSprites;

    [Header("Character Animations")]
    [SerializeField] private CharacterAnimation jumpOnTargetAnimation;
    [SerializeField] private CharacterAnimation idleSpriteAnim;
    private CharacterAnimationType currentAnimation = CharacterAnimationType.None;

    public void SetCharacter(CharacterScriptable_Battle character)
    {
        characterSprite.sprite = character.GameSprite();

        if (character.HandSprite != null)
        {
            foreach (SpriteRenderer spr in handsSprites)
            {
                spr.sprite = character.HandSprite;

                if(character.DisplayHand)
                {
                    spr.gameObject.SetActive(true);
                }
            }
        }

        SetAnimation(CharacterAnimationType.Idle);
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

    public void SetWalkAnimation(bool value)
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
                jumpOnTargetAnimation.Play(launchedSpell.targetNode.worldPosition);
                break;
            default:
                SetAnimation(launchedSpell.scriptable.CastingAnimation);
                TimerManager.CreateGameTimer(launchedSpell.scriptable.CastDuration, () => SetAnimation(CharacterAnimationType.LaunchSpell));
                break;
        }
    }

    protected void SetAnimation(CharacterAnimationType toSet)
    {
        if (toSet != currentAnimation)
        {
            UnsetAnimation(currentAnimation);

            currentAnimation = toSet;
            switch(toSet)
            {
                case CharacterAnimationType.Idle:
                    AnimSetBool("IsIdle", true);
                    idleSpriteAnim.Play(Vector2.zero);
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
    }
}
