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

    public GameObject inventoryPanel;
    public GameObject bagPanel;
    public GameObject equipmentPanel;

    [Space]
    public UI_InteractionPanel interactionPanel;

    [Space]
    public Transform bagItemPanelsParent;
    public UI_BagInternalPanels[] bagItemSlotsParent;

    public Dictionary<Panels, GameObject> panelsDictionary;

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

    public void SwitchTo(Panels panelToOPen)
    {
        foreach (var panel in panelsDictionary.Keys)
        {
            panelsDictionary[panel].SetActive(false);

            if(panel == panelToOPen)
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
}
