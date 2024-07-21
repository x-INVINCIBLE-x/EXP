using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SelectionSlot : UI_ItemSlot
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (!itemImage.sprite)
            return;

        UI_SelectionSlotHandler currHandler = GetComponentInParent<UI_SelectionSlotHandler>();
        currHandler.gameObject.SetActive(false);
        Inventory.Instance.EquipItem(item.data, currHandler.parentSlot);
    }
}
