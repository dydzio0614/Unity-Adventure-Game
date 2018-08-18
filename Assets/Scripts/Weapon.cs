using UnityEngine;
using System.Collections;
public enum DamageType
{
    Slashing,
    Bludgeon,
    Piercing
}

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public int damage;
    public DamageType damageType;
    public GameObject parent;

    public abstract void Attack();
}
