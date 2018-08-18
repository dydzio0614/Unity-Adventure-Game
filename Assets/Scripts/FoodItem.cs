using UnityEngine;
using System.Collections;

[CreateAssetMenu]
[System.Serializable]
public class FoodItem : InventoryItem
{
    public int HealAmount;

    public override void Use()
    {
        Owner.GetComponent<LivingObject>().Heal(HealAmount);
        Owner.GetComponent<Player>().Inventory.Remove(this);
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>().DeleteItems();
        Owner.GetComponent<Player>().RefreshInventoryItems();
    }

    public override string ToString()
    {
        return Name + " (click to use)" + System.Environment.NewLine + "Heal value: " + HealAmount + System.Environment.NewLine + System.Environment.NewLine + Description;
    }
}
