using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UI_Tab : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string tabName;
    [SerializeField] [TextArea] private string tabDescription;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.instance.panelToolTip.ShowToolTip(tabName, tabDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI.instance.panelToolTip.HideToolTip();
    }
}
