using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public enum Panels
{
    inventoryPanel,
    bagPanel,
    equipmentPanel
}

public class UI : MonoBehaviour
{
    public static UI instance;

    [Header("Panels")]
    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    public GameObject bagPanel;

    [Space]
    public UI_InteractionPanel interactionPanel;
    public Dictionary<Panels, GameObject> panelsDictionary;

    [Space]
    [Header("Bag Details")]
    public Transform bagItemPanelsParent;
    public UI_BagInternalPanels[] bagItemSlotsParent;


    [Header("ToolTip Details")]
    public UI_ItemToolTip itemToolTip;
    public UI_EquipmentToolTip equipmentToolTip;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        panelsDictionary = new Dictionary<Panels, GameObject>();

        panelsDictionary[Panels.inventoryPanel] = inventoryPanel;
        panelsDictionary[Panels.equipmentPanel] = equipmentPanel;
        panelsDictionary[Panels.bagPanel] = bagPanel;

        bagItemSlotsParent = GetComponentsInChildren<UI_BagInternalPanels>(true);
    }

    private void Start()
    {
        interactionPanel.Hide();
    }

    public void SwitchTo(Panels panelToOpen)
    {
        foreach (var panel in panelsDictionary.Keys)
        {
            HideToolTips();
            panelsDictionary[panel].SetActive(false);

            if(panel == panelToOpen)
                panelsDictionary[panel].SetActive(true);
        }
    }

    public void EnableInteractionPanel(Transform itemSlotTransform, bool canUse, bool canMoveToEquipment, bool canBeDestroyed, ItemData_Usable usableItem = null)
    {
        interactionPanel.Setup(canUse, canMoveToEquipment, canBeDestroyed, usableItem);
        interactionPanel.Show(itemSlotTransform);
    }

    public void SwitchBagPanel(UI_BagInternalPanels panelToOpen)
    {
        foreach (UI_BagInternalPanels bagPanel in bagItemSlotsParent)
        {
            bagPanel.gameObject.SetActive(false);

            if (bagPanel == panelToOpen)
            {
                bagPanel.gameObject.SetActive(true);

            }
        }
    }

    public void ShowToolTip(ItemData item)
    {
        HideToolTips();

        if (item.itemType == ItemType.Equipment)
            equipmentToolTip.ShowToolTip(item);
        else
            itemToolTip.ShowToolTip(item);
    }

    public void HideToolTips()
    {
        itemToolTip.HideToolTip();
        equipmentToolTip.HideToolTip();
    }
}
