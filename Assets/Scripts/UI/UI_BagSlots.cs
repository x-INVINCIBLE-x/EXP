using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BagSlots : UI_ItemSlot
{
    [SerializeField] private bool canUse;
    [SerializeField] private bool canMoveToEquipment;
    [SerializeField] private bool caBeDestroyed;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if(item.data != null)
            UI.instance.EnableInteractionPanel(transform, canUse, canMoveToEquipment, caBeDestroyed, item.data as ItemData_Usable);
    }
}
