using UnityEngine;
using System.Collections;

public class PickableObject : MonoBehaviour
{
    /* for ItemData inheritance and filling fields in inspector I have 4 options:
    1. create big Item Data class with fields that can handle all kinds of items - cons: potential trashdata in all items
    2. create subclasses of pickable object for each item type - cons: code quality - extra classes
    3. create tons of scriptable objects for every item prefab - chosen way - cons: asset file for each item
    4. create custom unity editor extension - seems to be overwork for current situation*/
    /// <summary>
    /// Field containing item parameters.
    /// </summary>
    public InventoryItem ItemData;

    private UIController UI;

    void Start()
    {
        UI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ItemData.Owner = other.gameObject;
            other.gameObject.GetComponent<Player>().Inventory.Add(ItemData);
            UI.DisplayQuickMessage("You find: " + ItemData.Name, 3);
            Destroy(gameObject);
        }
    }
}
