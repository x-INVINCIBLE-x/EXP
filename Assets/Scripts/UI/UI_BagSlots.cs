using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BagSlots : UI_ItemSlot
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        ItemData data = item.data;

        bool isEquipable = data.itemType == ItemType.Equipment || data.itemType == ItemType.UsableItem;


        if (itemImage.sprite)
            UI.instance.EnableInteractionPanel(transform, data.itemType == ItemType.UsableItem, isEquipable,data.canBeDestroyed, data);

        UI.instance.ShowToolTip(item.data);
    }
}
