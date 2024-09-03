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

    [SerializeField] private GameObject HUD;

    #region Inventory
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
    public UI_EquipmentToolTip selectionToolTip;
    public UI_PanelToolTip panelToolTip;

    private UI_ItemSlot lastSlotSelected;
    private List<UI_Panel> activePanels = new();
    [SerializeField] private InputManager inputManager;

    [SerializeField] private GameObject[] selectedSlotVisualizer;
    #endregion

    [Header("Portal Core Info")]
    public GameObject portalUI;

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
        HideToolTips();
    }

    private void OnEnable()
    {
        inputManager = InputManager.Instance;
        inputManager.BackEvent += ClosePanel;
    }

    private void OnDisable()
    {
        inputManager.BackEvent -= ClosePanel;
    }

    #region Inventory Functions
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
    public void EnableInteractionPanel(Transform itemSlotTransform, bool canUse, bool canMoveToEquipment, bool canBeDestroyed, ItemData item = null)
    {
        interactionPanel.Setup(canUse, canMoveToEquipment, canBeDestroyed, item);
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

    public void ShowToolTip(ItemData item, bool isSelecting = false)
    {
        if (item.itemType == ItemType.UsableItem)
        {
            itemToolTip.ShowToolTip(item);
            return;
        }

        if (isSelecting)
        {
            selectionToolTip.HideToolTip();
            selectionToolTip.ShowToolTip(item);
            return;
        }

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
        selectionToolTip.HideToolTip();
        panelToolTip.HideToolTip();
    }

    public void SelectSlot(UI_ItemSlot newSlot)
    {
        lastSlotSelected?.UnSelect();
        lastSlotSelected = newSlot;
        Inventory.Instance.UpdateSelectedSlot(newSlot);
    }

    public void DeselectSlot()
    {
        lastSlotSelected?.UnSelect();
    }

    public void AddToActivePanels(UI_Panel panel) => activePanels.Add(panel);
    public void RemoveFromActivePanel(UI_Panel panel) => activePanels.Remove(panel);

    public void ActivateSlotVisualizer(BeltType type)
    {
        if (type == BeltType.UpperBelt)
        {
            selectedSlotVisualizer[1].SetActive(false);
            selectedSlotVisualizer[0].SetActive(true);
        }
        else
        {
            selectedSlotVisualizer[0].SetActive(false);
            selectedSlotVisualizer[1].SetActive(true);
        }
    }
    public void ClosePanel()
    {
        HideToolTips();
        if (activePanels.Count == 0)
        {
            OpenInventory();
            return;
        }

        activePanels[activePanels.Count - 1].gameObject.SetActive(false);
        Inventory.Instance.UpdateSelectedSlot(null);

        if(activePanels.Count == 0)
            HUD.SetActive(true);
    }

    public void OpenInventory()
    {
        HUD.SetActive(false);
        inventoryPanel.SetActive(true);
    }

    public bool hasActivePanels() => activePanels.Count > 0;
    #endregion

    public void SetPortalUI(bool active) => portalUI.SetActive(active);
}
