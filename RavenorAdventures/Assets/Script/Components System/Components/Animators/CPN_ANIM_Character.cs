using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterAnimation
{
    Idle,
    Walk,
    CastSpell,
    LaunchSpell
}

public class CPN_ANIM_Character : CPN_AnimationHandler
{
    private CharacterAnimation currentAnimation;

    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Texture normalMap;
    private Material mat;

    private void Start()
    {
        mat = Instantiate(renderer.material);
        mat.SetTexture("NormalMap", normalMap);
        renderer.material = mat;
        renderer.sprite = sprite;
    }

    public void SetWalkAnimation(bool value)
    {
        if(value)
        {
            SetAnimation(CharacterAnimation.Walk);
        }
        else
        {
            UnsetAnimation(CharacterAnimation.Walk);
        }
    }

    public void SetCastSpellAnimation(CharacterAnimation toPlay, float castTime)
    {
        SetAnimation(toPlay);

        TimerManager.CreateGameTimer(castTime, () => SetAnimation(CharacterAnimation.LaunchSpell));
    }

    protected void SetAnimation(CharacterAnimation toSet)
    {
        if(toSet != currentAnimation)
        {
            UnsetAnimation(currentAnimation);

            currentAnimation = toSet;
            switch(toSet)
            {
                case CharacterAnimation.Idle:
                    AnimSetBool("IsIdle", true);
                    break;
                case CharacterAnimation.Walk:
                    AnimSetBool("IsWalking", true);
                    break;
                case CharacterAnimation.CastSpell:
                    AnimSetBool("IsCasting", true);
                    break;
                case CharacterAnimation.LaunchSpell:
                    animator.SetTrigger("IsSpellLaunch");
                    break;
            }
        }
    }

    protected void UnsetAnimation(CharacterAnimation toUnset)
    {
        if(toUnset == currentAnimation)
        {
            currentAnimation = CharacterAnimation.Idle;
            switch (toUnset)
            {
                case CharacterAnimation.Idle:
                    AnimSetBool("IsIdle", false);
                    break;
                case CharacterAnimation.Walk:
                    AnimSetBool("IsWalking", false);
                    break;
                case CharacterAnimation.CastSpell:
                    AnimSetBool("IsCasting", false);
                    break;
                case CharacterAnimation.LaunchSpell:
                    AnimSetBool("IsSpellLaunch", false);
                    break;
            }
        }
    }
}
