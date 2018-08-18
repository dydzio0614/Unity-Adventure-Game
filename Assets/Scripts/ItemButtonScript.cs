using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ItemButtonScript : MonoBehaviour, IPointerEnterHandler
{
    UIController UI;
    Player PlayerData;

    void Start()
    {
        UI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
        PlayerData = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.DisplayItemDetails(PlayerData.Inventory[transform.GetSiblingIndex()]);
        //Debug.Log(transform.GetSiblingIndex());
    }
}
