using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterArmorDisplayer : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetArmor(bool isSet)
    {
        animator.SetBool("IsSet", isSet);
    }

    public void GainArmor()
    {
        animator.SetBool("IsFull", true);
    }

    public void LoseArmor()
    {
        animator.SetBool("IsFull", false);
    }
}
