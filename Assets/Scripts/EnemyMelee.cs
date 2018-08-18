using UnityEngine;
using System.Collections;
using System;

public class EnemyMelee : Enemy
{
    Weapon weaponScript;

    void Start()
    {
        InitializeEnemy();
        weaponScript = GetComponentInChildren<Weapon>();
	}
	
	void Update()
    {
        CheckPlayerState(25, 1.5f);
    }

    protected override void PerformAttack()
    {
        weaponScript.Attack();
        Vector3 target = player.transform.position - transform.position;
        target.y = 0;
        transform.rotation = Quaternion.LookRotation(target);
    }
}
