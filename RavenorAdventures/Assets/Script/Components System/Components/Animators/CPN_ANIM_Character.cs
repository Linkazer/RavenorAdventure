using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterAnimationType
{
    Idle,
    Walk,
    CastSpell,
    LaunchSpell,
    JumpOnTarget
}

public class CPN_ANIM_Character : CPN_AnimationHandler
{
    [Header("Character Display")]
    [SerializeField] private SpriteRenderer characterSprite;

    [Header("Character Animations")]
    [SerializeField] private CharacterAnimation jumpOnTargetAnimation;
    private CharacterAnimationType currentAnimation;

    public void SetCharacter(CharacterScriptable character)
    {
        characterSprite.sprite = character.GameSprite();
    }

    public void SetOrientation(Vector2 direction)
    {
        if(direction.x > 0)
        {
            characterSprite.flipX = false;
        }
        else if(direction.x < 0)
        {
            characterSprite.flipX = true;
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
            UnsetAnimation(CharacterAnimationType.Walk);
        }
    }

    public void SetCastSpellAnimation(LaunchedSpellData launchedSpell)
    {
        switch(launchedSpell.scriptable.CastingAnimation)
        {
            case CharacterAnimationType.CastSpell:
                SetAnimation(launchedSpell.scriptable.CastingAnimation);
                TimerManager.CreateGameTimer(launchedSpell.scriptable.CastDuration, () => SetAnimation(CharacterAnimationType.LaunchSpell));
                break;
            case CharacterAnimationType.JumpOnTarget:
                jumpOnTargetAnimation.Play(launchedSpell.targetNode.worldPosition);
                break;
        }
    }

    protected void SetAnimation(CharacterAnimationType toSet)
    {
        if(toSet != currentAnimation)
        {
            UnsetAnimation(currentAnimation);

            currentAnimation = toSet;
            switch(toSet)
            {
                case CharacterAnimationType.Idle:
                    AnimSetBool("IsIdle", true);
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
            }
        }
    }
}
