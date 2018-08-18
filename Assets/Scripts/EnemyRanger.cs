using UnityEngine;
using System.Collections;

public class EnemyRanger : Enemy
{

    //public GameObject weapon;
    public GameObject EquippedArrow;
    public GameObject ArrowPrefab;

    private bool isShooting;

    void Start()
    {
        InitializeEnemy();
    }
	
	void Update()
    {
        CheckPlayerState(30, 20);
    }

    protected override void PerformAttack()
    {
        if (!isShooting)
            StartCoroutine(Shoot());
        humanAnimator.SetBool("run", false);
        Vector3 target = player.transform.position - transform.position;
        target.y = 0;
        transform.rotation = Quaternion.LookRotation(target);
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        EquippedArrow.SetActive(true);

        humanAnimator.SetTrigger("shoot");

        yield return new WaitForSeconds(0.6f);

        EquippedArrow.SetActive(false);

        Arrow arrow = Instantiate(ArrowPrefab).GetComponent<Arrow>();
        arrow.parent = gameObject;
        arrow.Attack();
        yield return new WaitForSeconds(0.4f);
        isShooting = false;
    }
}
