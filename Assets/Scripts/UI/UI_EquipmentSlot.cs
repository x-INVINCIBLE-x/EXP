using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipmentSlot : UI_ItemSlot
{
    [SerializeField] protected GameObject selectionUI;
    [SerializeField] private EquipmentType equipmentType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        UI_SelectionSlotHandler slotHandler = selectionUI.GetComponent<UI_SelectionSlotHandler>();
        slotHandler.parentSlot = this;

        if(eventData.clickCount == 0)
        {
            if (itemImage.sprite != defaultImage)
                UI.instance.ShowToolTip(item.data);
            return;
        }

        Inventory.Instance.UpdateSelectionSlotUI(equipmentType);
        selectionUI.SetActive(true);
    }

    public override void CleanUpSlot()
    {
        base.CleanUpSlot();

        if(!defaultImage)
            return;

        itemImage.sprite = defaultImage;
        itemImage.color = Color.white;
    }

    public override void RestoreState(object state)
    {
        base.RestoreState(state);
    }

}
