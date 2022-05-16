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
    private CharacterAnimationType currentAnimation;
    [SerializeField] private CharacterAnimation jumpOnTargetAnimation;

    /*[SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Texture normalMap;

    private Material mat;*/

    private void Start()
    {
        /*mat = Instantiate(renderer.material);
        mat.SetTexture("NormalMap", normalMap);
        renderer.material = mat;
        renderer.sprite = sprite;*/
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

    public void SetCastSpellAnimation(CharacterAnimationType toPlay, float castTime)
    {
        SetAnimation(toPlay);

        TimerManager.CreateGameTimer(castTime, () => SetAnimation(CharacterAnimationType.LaunchSpell));
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
