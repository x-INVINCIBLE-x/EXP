using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    [SerializeField] protected GameObject selectionUI;

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


        selectionUI.SetActive(true);
    }

    public void EquipItem(ItemData item)
    {

    }

    public void UnequipItem()
    {
         
    }

    public override void CleanUpSlot()
    {
        base.CleanUpSlot();

        if(!defaultImage)
            return;

        itemImage.sprite = defaultImage;
        itemImage.color = Color.white;
    }
}
