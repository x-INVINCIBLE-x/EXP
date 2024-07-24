using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BagSlots : UI_ItemSlot
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        bool isEquipable = item.data.itemType == ItemType.Equipment || item.data.itemType == ItemType.UsableItem;


        if (itemImage.sprite)
            UI.instance.EnableInteractionPanel(transform, item.data.itemType == ItemType.UsableItem, isEquipable,item.data.canBeDestroyed, item.data as ItemData_Usable);
    }
}
