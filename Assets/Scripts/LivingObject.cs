using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for all objects that can move, have hitpoints and can take damage. Does not have to be able to attack.
/// </summary>
public abstract class LivingObject : MonoBehaviour
{
    protected CharacterController objectController;

    [SerializeField]
    private CreatureData objectData; //make this protected?

    protected Vector3 spawnLocation;

    protected float bodyLastingTime = 30f;

    protected bool isDead;

    protected int HP { get { return objectData.HitPoints; } }

    protected int maxHP;

    protected string statsVerbal { get { return objectData.ToString(); } }

    /// <summary>
    /// Returns current percentage of object health compared to its maximum as a fraction at range [0, 1]
    /// </summary>
    public float RelativeHealth
    {
        get
        {
            if (maxHP == 0) maxHP = HP;
            return (float)HP / maxHP;
        }
    }

    public string LivingObjectName
    {
        get
        {
            return objectData.Name;
        }
    }

    public void Heal(int amount)
    {
        objectData.HitPoints += amount;
        if (objectData.HitPoints > maxHP)
            objectData.HitPoints = maxHP;
    }

    protected virtual void TakeDamage(int damage)
    {
        if (damage < 0) damage = 0;
        objectData.HitPoints -= damage;
        if (objectData.HitPoints <= 0)
            Die(bodyLastingTime);
    }

    protected virtual void Die(float delay)
    {
        Destroy(gameObject, delay);
        isDead = true;
    }

    protected virtual void MoveForward(float speed)
    { 
        objectController.Move(transform.forward * Time.deltaTime * speed);
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        //Weapon collidingWeapon = collider.gameObject.GetComponent<Weapon>(); //not compatible with bow weapon (new arrow prefab breaking change)
        Weapon collidingWeapon = collider.gameObject.GetComponentInParent<Weapon>();
        if (collidingWeapon != null && collidingWeapon.parent != gameObject && !isDead)
        {
            collidingWeapon.GetComponentInChildren<Collider>().enabled = false;

            if (collidingWeapon.damageType == DamageType.Bludgeon) //simplify?
                TakeDamage(collidingWeapon.damage - objectData.BludgeonDR);
            else if (collidingWeapon.damageType == DamageType.Piercing)
                TakeDamage(collidingWeapon.damage - objectData.PiercingDR);
            else if (collidingWeapon.damageType == DamageType.Slashing)
                TakeDamage(collidingWeapon.damage - objectData.SlashingDR);

            if (collidingWeapon is Arrow)
                Destroy(collidingWeapon.gameObject);
        }
    }
}
