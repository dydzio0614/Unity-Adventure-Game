using UnityEngine;
using System.Collections;

public class LivingHumanoid : LivingObject
{
    protected Animator humanAnimator;
    public float Speed;
    public float JumpHeight;

    protected void DisableWeaponCollider() // called by attack3front animation event
    {
        Weapon currentWeapon = GetComponentInChildren<Weapon>();
        if(currentWeapon != null)
        {
            currentWeapon.GetComponentInChildren<Collider>().enabled = false;
        }
    }

    protected void EnableWeaponCollider() // called by attack3front animation event
    {
        Weapon currentWeapon = GetComponentInChildren<Weapon>();
        if (currentWeapon != null)
        {
            currentWeapon.GetComponentInChildren<Collider>().enabled = true;
        }
    }

    protected override void MoveForward(float speed) //refactor code (enemy, interactivehumanoid) to benefit from MoveForward(0), or scrap MoveForward(0) behavior
    {
        if (speed != 0)
        {
            humanAnimator.SetBool("run", true);
            base.MoveForward(speed);
            if (objectController.velocity.magnitude < 2 && objectController.isGrounded)
                Jump(JumpHeight);
        }
        else
            humanAnimator.SetBool("run", false);
    }

    protected virtual void Jump(float height)
    {
        StartCoroutine(JumpCoroutine(height));
    }

    protected virtual IEnumerator JumpCoroutine(float height)
    {
        float time = 0.4f;

        humanAnimator.SetBool("jump", true);

        while (time > 0)
        {
            objectController.Move(Vector3.up * height * Time.deltaTime); //deltatime in both timer and move so jump isnt higher with better framerate
            time -= Time.deltaTime;
            yield return null;
        }
        humanAnimator.SetBool("jump", false);
    }

    protected override void TakeDamage(int damage)
    {
        humanAnimator.SetTrigger("hit");
        base.TakeDamage(damage);
    }

    protected override void Die(float delay)
    {
        base.Die(delay);
        humanAnimator.SetTrigger("die"); 
    }
}
