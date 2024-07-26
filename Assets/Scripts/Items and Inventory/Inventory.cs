using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance {  get; private set; }

    public List<InventoryItem> usableItems = new();
    public Dictionary<ItemData_Usable, InventoryItem> usableItemsDictionary = new();

    public List<InventoryItem> materials = new();
    public Dictionary<ItemData, InventoryItem> materialsDictionary = new();

    public List<InventoryItem> weapons = new();

    public List<InventoryItem> defenceParts = new();
    public Dictionary<ItemData_Equipment, InventoryItem> defencePartsDictionary = new();

    public List<InventoryItem> amulets = new();
    public Dictionary<ItemData_Equipment, InventoryItem> amuletsDictionary = new();

    public List<ItemData> startingItems;

    public Transform usableItemsParent;
    public Transform materialsParent;
    public Transform defencePartsParent;
    public Transform amuletsParent;

    public Transform frameSelectionParent;
    public Transform convertorSelectionParent;
    public Transform cartilidgeSelectionParent;
    public Transform linearSelectionParent;
    public Transform amuletSelectionParent;

    private UI_ItemSlot[] usableItemsSlots;
    private UI_ItemSlot[] materialSlots;
    [SerializeField] private UI_EquipmentSlot[] defencePartsSlots;
    [SerializeField] private UI_EquipmentSlot[] amuletSlots;

    [SerializeField] private UI_SelectionSlot[] frameSelectionSlots;
    [SerializeField] private UI_SelectionSlot[] convertorSelectionSlots;
    [SerializeField] private UI_SelectionSlot[] cartidgeSelectionSlots;
    [SerializeField] private UI_SelectionSlot[] linearSelectionSlots;
    [SerializeField] private UI_SelectionSlot[] amuletSelectionSlots;

    [Header("Bag Panel Parents")]
    public Transform bagSlotsParent;

    [Header("Bag Panel Slots")]
    [SerializeField] [HideInInspector] private UI_BagSlots[] bagSlots;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        usableItemsSlots = usableItemsParent.GetComponentsInChildren<UI_ItemSlot>();
        materialSlots = materialsParent.GetComponentsInChildren<UI_ItemSlot>();
        defencePartsSlots = defencePartsParent.GetComponentsInChildren<UI_EquipmentSlot>();
        amuletSlots = amuletsParent.GetComponentsInChildren<UI_EquipmentSlot>();

        frameSelectionSlots = frameSelectionParent.GetComponentsInChildren<UI_SelectionSlot>(true);
        convertorSelectionSlots = convertorSelectionParent.GetComponentsInChildren<UI_SelectionSlot>(true);
        cartidgeSelectionSlots = cartilidgeSelectionParent.GetComponentsInChildren<UI_SelectionSlot>(true);
        linearSelectionSlots = linearSelectionParent.GetComponentsInChildren<UI_SelectionSlot>(true);
        amuletSelectionSlots = amuletSelectionParent.GetComponentsInChildren<UI_SelectionSlot>(true);

        bagSlots = bagSlotsParent.GetComponentsInChildren<UI_BagSlots>(true);
    }

    private void Start()
    {
        AddStartingItems();
        ShowBagItemSlots(ItemType.UsableItem);
        UpdateSlotUI();
    }

    public void AddStartingItems()
    {
        for (int i = 0; i < startingItems.Count; i++)
            AddItem(startingItems[i]);
    }

    #region Add Item to Inventory
    public void AddItem(ItemData item)
    {
        if(item.itemType == ItemType.UsableItem)
            AddItem(item, ref usableItems, ref usableItemsDictionary);
        if (item.itemType == ItemType.Material)
            AddItem(item, ref materials, ref materialsDictionary);
        if (item.itemType == ItemType.Equipment)
            AddToEquipments(item);
    }

    private void AddToEquipments(ItemData item)
    {
        ItemData_Equipment equipment = item as ItemData_Equipment;

        if (equipment == null) return;

        if(equipment.equipmentType == EquipmentType.Amulet)
            AddItem(item, ref amulets, ref amuletsDictionary);
        if (equipment.equipmentType == EquipmentType.Defence)
            AddItem(item, ref defenceParts, ref defencePartsDictionary);
    }

    public void AddItem(ItemData item, ref List<InventoryItem> itemList, ref Dictionary<ItemData, InventoryItem> itemDictionary) 
    {
        if(itemDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            itemList.Add(newItem);
            itemDictionary[item] = newItem;
        }
    }

    public void AddItem(ItemData item, ref List<InventoryItem> itemList, ref Dictionary<ItemData_Equipment, InventoryItem> itemDictionary)
    {
        ItemData_Equipment equipmwnt = item as ItemData_Equipment;

        if (itemDictionary.TryGetValue(equipmwnt, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(equipmwnt);
            itemList.Add(newItem);
            itemDictionary[equipmwnt] = newItem;
        }
    }

    public void AddItem(ItemData item, ref List<InventoryItem> itemList, ref Dictionary<ItemData_Usable, InventoryItem> itemDictionary)
    {
        ItemData_Usable usableItem = item as ItemData_Usable;

        if (itemDictionary.TryGetValue(usableItem, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(usableItem);
            itemList.Add(newItem);
            itemDictionary[usableItem] = newItem;
        }
    }
    #endregion
    #region Equip/ Unequip
    public void EquipItem(ItemData item, UI_ItemSlot itemSlot)
    {
        ItemData_Equipment newEquipment = item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        newEquipment.AddModifiers();
        if (newEquipment.subEquipmentType == EquipmentType.Defence)
        {
            defenceParts.Add(newItem);
            defencePartsDictionary[newEquipment] = newItem;
        }

        if (itemSlot.item != null)
        {
            UnequipItem(itemSlot);
        }

        itemSlot.item = newItem;
        itemSlot.UpdateSlot(newItem);
    }

    public void UnequipItem(UI_ItemSlot itemSlot)
    {
        ItemData_Equipment item = itemSlot.item.data as ItemData_Equipment;
        if (item == null)
        {
            Debug.LogWarning("ItemData_Equipment not found!");
            return;
        }

        item.RemoveModifiers();
        itemSlot.CleanUpSlot();
    }
    #endregion
    #region Remove Item From Inventory
    public void RemoveItem(ItemData item)
    {
        if (materialsDictionary.TryGetValue(item, out InventoryItem value))
        {
            RemoveItem(value, ref materials, ref materialsDictionary);
            return;
        }
        
        ItemData_Equipment equipment = item as ItemData_Equipment;
        if (equipment != null)
        {
            if (amuletsDictionary.TryGetValue(equipment, out value))
                RemoveItem(value, ref amulets, ref amuletsDictionary);

            if(defencePartsDictionary.TryGetValue(equipment, out value))
                RemoveItem(value, ref defenceParts, ref defencePartsDictionary);
        }

        ItemData_Usable usableItem = item as ItemData_Usable;
        if(usableItem != null)
        {
             if (usableItemsDictionary.TryGetValue(usableItem, out value))
                RemoveItem(value, ref usableItems, ref usableItemsDictionary);
        }

    }

    public void RemoveItem(InventoryItem item, ref List<InventoryItem> itemLIst, ref Dictionary<ItemData, InventoryItem> itemDictionary)
    {
        if (item.stackSize == 1)
        {
            itemLIst.Remove(item);
            itemDictionary.Remove(item.data);
        }
        else
            item.RemoveStack();

        UpdateSlotUI();
        ShowBagItemSlots(item.data.itemType);
    }

    public void RemoveItem(InventoryItem item, ref List<InventoryItem> itemLIst, ref Dictionary<ItemData_Equipment, InventoryItem> itemDictionary)
    {
        ItemData_Equipment equipment = item.data as ItemData_Equipment;
        if (item.stackSize == 1)
        {
            itemLIst.Remove(item);
            itemDictionary.Remove(equipment);
        }
        else
            item.RemoveStack();

        UpdateSlotUI();
        ShowBagItemSlots(equipment.itemType, equipment.equipmentType);
    }

    public void RemoveItem(InventoryItem item, ref List<InventoryItem> itemLIst, ref Dictionary<ItemData_Usable, InventoryItem> itemDictionary)
    {
        if (item.stackSize == 1)
        {
            itemLIst.Remove(item);
            itemDictionary.Remove(item.data as ItemData_Usable);
        }
        else
            item.RemoveStack();

        UpdateSlotUI();
        ShowBagItemSlots(item.data.itemType);
    }


    #endregion

    #region Bag UI Update

    public void ShowBagItemSlots(ItemType itemType, EquipmentType equipmentType = EquipmentType.None)
    {

        foreach (var slot in bagSlots)
        {
            slot.CleanUpSlot();
        }

        if (itemType == ItemType.Equipment)
            ShowBagEquipmentSlots(itemType, equipmentType);

        else if (itemType == ItemType.Material)
            DisplayBagSlots(materials);

        else if (itemType == ItemType.UsableItem)
            DisplayBagSlots(usableItems);
    }

    private void ShowBagEquipmentSlots(ItemType itemType, EquipmentType equipmentType)
    {
        if (equipmentType == EquipmentType.Amulet)
        {
            DisplayBagSlots(amulets);
        }
        else if (equipmentType == EquipmentType.Defence)
        {
            DisplayBagSlots(defenceParts);
        }
    }

    #endregion

    #region Update UI
    public void DisplayBagSlots(List<InventoryItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            bagSlots[i].UpdateSlot(items[i]);
        }
    }

    public void UpdateSlotUI()
    {
        CleanSlots();

        int i = 0, j = 0, k = 0, l = 0;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> defencePart in defencePartsDictionary)
        {
            ItemData_Equipment key = defencePart.Key;
            InventoryItem item = defencePart.Value;

            if (key.subEquipmentType == EquipmentType.Frame)
                frameSelectionSlots[i++].UpdateSlot(item);

            else if (key.subEquipmentType == EquipmentType.Converter)
                convertorSelectionSlots[j++].UpdateSlot(item);

            else if (key.subEquipmentType == EquipmentType.Cartridge)
                cartidgeSelectionSlots[k++].UpdateSlot(item);

            else if (key.subEquipmentType == EquipmentType.Linear)
                linearSelectionSlots[l++].UpdateSlot(item);
        }

        for (i = 0; i < amulets.Count; i++)
            amuletSelectionSlots[i].UpdateSlot(amulets[i]);
    }

    private void CleanSlots()
    {
        for (int i = 0; i < amuletSlots.Length; i++)
            amuletSlots[i].CleanUpSlot();

        for (int i = 0; i < defencePartsSlots.Length; i++)
            defencePartsSlots[i].CleanUpSlot();

        for (int i = 0; i < frameSelectionSlots.Length; i++)
            frameSelectionSlots[i].CleanUpSlot();

        for (int i = 0; i < convertorSelectionSlots.Length; i++)
            convertorSelectionSlots[i].CleanUpSlot();

        for (int i = 0; i < cartidgeSelectionSlots.Length; i++)
            cartidgeSelectionSlots[i].CleanUpSlot();

        for (int i = 0; i < linearSelectionSlots.Length; i++)
            linearSelectionSlots[i].CleanUpSlot();

        for (int i = 0; i < amuletSelectionSlots.Length; i++)
            amuletSelectionSlots[i].CleanUpSlot();
    }

    #endregion
}
