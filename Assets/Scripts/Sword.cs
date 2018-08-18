using UnityEngine;
using System.Collections;
using System;

public class Sword : Weapon
{
    Animator wielderAnimator;

    void Start()
    {
        wielderAnimator = parent.GetComponent<Animator>();
    }

    public override void Attack()
    {
        if (!wielderAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack"))
        {
            wielderAnimator.SetTrigger("slash");
        }
    }
}
