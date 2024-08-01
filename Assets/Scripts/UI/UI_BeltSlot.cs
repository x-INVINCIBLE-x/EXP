using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BeltType
{
    UpperBelt,
    LowerBelt,
    Extra
}

public class UI_BeltSlot : UI_ItemSlot
{
    public GameObject selectionUI;
    public BeltType type;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (eventData.clickCount < 1)
            return;

        UI_SelectionSlotHandler slotHandler = selectionUI.GetComponent<UI_SelectionSlotHandler>();
        slotHandler.parentSlot = this;

        Inventory.Instance.UpdateSelectionSlotUI(ItemType.UsableItem);
        selectionUI.SetActive(true);
    }

    public override void CleanUpSlot()
    {
        base.CleanUpSlot();

        if (!defaultImage)
            return;

        itemImage.sprite = defaultImage;
        itemImage.color = Color.white;
    }
}
