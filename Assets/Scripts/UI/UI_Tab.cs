using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Tab : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject tabToOpen;

    [SerializeField] private ItemType itemType;
    [SerializeField] private EquipmentType equipmentType;
    public void OnPointerDown(PointerEventData eventData)
    {
        UI.instance.DeselectSlot();

        if (tabToOpen.TryGetComponent(out UI_BagInternalPanels internalPanel))
        {
            //UI.instance.SwitchBagPanel(internalPanel);
            UI.instance.HideToolTips();
            Inventory.Instance.ShowBagItemSlots(itemType, equipmentType);
            return;
        }

        tabToOpen.SetActive(true);
    }
}
