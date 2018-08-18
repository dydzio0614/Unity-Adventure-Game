using UnityEngine;
using System.Collections;

public enum ItemType
{
    Weapon,
    Food,
    Ammo,
    Misc
}
[CreateAssetMenu]
[System.Serializable]
public class InventoryItem : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
    public GameObject Owner;
    public string Description;

    /// <summary>
    /// Virtual function determining action on item click in inventory
    /// </summary>
    public virtual void Use()
    {

    }

    public override string ToString()
    {
        return Name + System.Environment.NewLine + System.Environment.NewLine + Description; //there is probably no need for stringbuilder in any item subclass
    }
}


