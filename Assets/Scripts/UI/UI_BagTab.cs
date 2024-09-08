using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BagTab : UI_Tab
{
    [SerializeField] private ItemType itemType;
    [SerializeField] private EquipmentType equipmentType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        UI.instance.DeselectSlot();
        if (nameDisplay)
            nameDisplay.text = tabName;

        //UI.instance.SwitchBagPanel(internalPanel);
        UI.instance.HideToolTips();
        Inventory.Instance.ShowBagItemSlots(itemType, equipmentType);
    }
}
