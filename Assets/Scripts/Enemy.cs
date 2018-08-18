using UnityEngine;
using System.Collections;

public abstract class Enemy : LivingHumanoid
{
    protected GameObject player;

    protected bool isNotAttacking { get { return (humanAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle") || humanAnimator.GetCurrentAnimatorStateInfo(0).IsName("runneutral")); } }

    float chaseTime;

    bool recentlyDamaged;

    int previousHP;

    /// <summary>
    /// Contains common code to be ran on enemy subclass initialization.
    /// </summary>
    protected void InitializeEnemy()
    {
        spawnLocation = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        humanAnimator = GetComponent<Animator>();
        objectController = GetComponent<CharacterController>();
        previousHP = maxHP;
    }

    protected abstract void PerformAttack();

    /// <summary>
    /// Main AI method determining what action to take every frame depending on player data.
    /// </summary>
    /// <param name="spotDistance">Distance at which AI notices incoming player.</param>
    /// <param name="attackDistance">Distance towards player at which AI uses weapon against player. No movement actions are taken by default during attack.</param>
    protected virtual void CheckPlayerState(float spotDistance, float attackDistance)
    {
        if (player == null)
        {
            chaseTime = 0;
            humanAnimator.SetBool("run", false);
            return;
        }

        if (isDead) return;

        if (HP < previousHP)
            recentlyDamaged = true;

        previousHP = HP;

        Vector3 target = player.transform.position - transform.position;
        target.y = 0;

        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
        {
            chaseTime = 0;
            PerformAttack();
        }

        else if ((Vector3.Distance(transform.position, player.transform.position) < spotDistance || recentlyDamaged) && chaseTime < 15f && isNotAttacking)
        {
            chaseTime += Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(target);
            MoveForward(Speed);
            humanAnimator.SetBool("run", true);
            humanAnimator.speed = 1.0f;
        }
        else if (Vector3.Distance(transform.position, spawnLocation) > 0.5f && isNotAttacking)
        {
            recentlyDamaged = false;
            transform.rotation = Quaternion.LookRotation(spawnLocation - transform.position);
            transform.position = Vector3.MoveTowards(transform.position, spawnLocation, 10 * Time.deltaTime);
            humanAnimator.speed = 1.5f;
            humanAnimator.SetBool("run", true);
        }
        else
        {
            chaseTime = 0;
            humanAnimator.SetBool("run", false);
        }
    }
}
