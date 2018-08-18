using UnityEngine;
using System.Collections;

public enum WeaponType
{
    Melee,
    Ranged
}

[CreateAssetMenu]
[System.Serializable]
class WeaponItem : InventoryItem
{
    public WeaponType weaponType;

    public Vector3 MeshLocationOffset;
    public Vector3 MeshRotationOffset;

    //public GameObject Prefab - exists in base class, needs to be used here

    public override void Use()
    {
        Player player = Owner.GetComponent<Player>();
        GameObject rHand = player.RightHand;
        GameObject lHand = player.LeftHand;

        foreach (Transform child in rHand.transform)
            if (child.tag == "Weapon")
                Destroy(child.gameObject);
        foreach (Transform child in lHand.transform)
            if (child.tag == "Weapon")
                Destroy(child.gameObject);

        GameObject newWeapon = Instantiate(Prefab);

        if (weaponType == WeaponType.Melee)
        {
            newWeapon.transform.SetParent(rHand.transform);
            newWeapon.GetComponent<Weapon>().parent = player.gameObject;
            player.Attack = player.MeleeAttack;
        }

        else if (weaponType == WeaponType.Ranged)
        {
            newWeapon.transform.SetParent(lHand.transform);
            player.Attack = player.RangedAttack;
        }
        //AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("holster2"), player.transform.position);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.transform.Translate(MeshLocationOffset);
        newWeapon.transform.Rotate(MeshRotationOffset);
    }

    public override string ToString()
    {
        Player player = Owner.GetComponent<Player>();
        int damage = 0;

        if (weaponType == WeaponType.Ranged)
        {
            damage = player.ArrowPrefab.GetComponent<Weapon>().damage;
        }
        else
            damage = Prefab.GetComponent<Weapon>().damage;


        return Name + " (click to equip)" + System.Environment.NewLine + System.Environment.NewLine + "Damage: " + damage + System.Environment.NewLine + Description;
    }
}
