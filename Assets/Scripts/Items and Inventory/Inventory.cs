using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public List<InventoryItem> usableItems = new();
    public Dictionary<ItemData_Usable, InventoryItem> usableItemsDictionary = new();

    public List<InventoryItem> materials = new();
    public Dictionary<ItemData, InventoryItem> materialsDictionary = new();

    public List<InventoryItem> weapons = new();
    public Dictionary<ItemData_Equipment, InventoryItem> weaponsDictionary = new();

    public List<InventoryItem> defenceParts = new();
    public Dictionary<ItemData_Equipment, InventoryItem> defencePartsDictionary = new();

    public List<InventoryItem> amulets = new();
    public Dictionary<ItemData_Equipment, InventoryItem> amuletsDictionary = new();

    public List<ItemData> startingItems;

    public Transform defencePartsParent;
    public Transform amuletsParent;
    public Transform weaponsParent;

    public Transform selectionSlotsParent;

    [SerializeField] private UI_EquipmentSlot[] defencePartsSlots;
    [SerializeField] private UI_EquipmentSlot[] amuletSlots;
    [SerializeField] private UI_EquipmentSlot[] weaponsSlots;

    [SerializeField] private UI_SelectionSlot[] selectionSlots;

    [Header("Bag Panel Parents")]
    public Transform bagSlotsParent;

    [Header("Bag Panel Slots")]
    [SerializeField][HideInInspector] private UI_BagSlots[] bagSlots;

    private UI_ItemSlot selectedSlot;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        selectionSlots = selectionSlotsParent.GetComponentsInChildren<UI_SelectionSlot>(true);

        defencePartsSlots = defencePartsParent.GetComponentsInChildren<UI_EquipmentSlot>();
        amuletSlots = amuletsParent.GetComponentsInChildren<UI_EquipmentSlot>();
        weaponsSlots = weaponsParent.GetComponentsInChildren<UI_EquipmentSlot>();

        bagSlots = bagSlotsParent.GetComponentsInChildren<UI_BagSlots>(true);
    }

    private void Start()
    {
        AddStartingItems();
        ShowBagItemSlots(ItemType.UsableItem);
        CleanSlots();
    }

    //Temporary Input Check
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (selectedSlot == null)
                return;

            if (selectedSlot.TryGetComponent(out UI_SelectionSlot selectionSlot))
            {
                ItemData_Equipment equipment = selectedSlot.item.data as ItemData_Equipment;
                if (equipment.isEquipped)
                    UnequipItem(equipment);

                UpdateSelectedSlot(null);
                return;
                
            }

            UnequipItem(selectedSlot);
            UpdateSelectedSlot(null);
        }
    }

    public void AddStartingItems()
    {
        for (int i = 0; i < startingItems.Count; i++)
            AddItem(startingItems[i]);
    }

    #region Add Item to Inventory
    public void AddItem(ItemData item)
    {
        if (item.itemType == ItemType.UsableItem)
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

        if (equipment.equipmentType == EquipmentType.Amulet)
            AddItem(item, ref amulets, ref amuletsDictionary);
        if (equipment.equipmentType == EquipmentType.Defence)
            AddItem(item, ref defenceParts, ref defencePartsDictionary);
        if (equipment.equipmentType == EquipmentType.Weapon)
            AddItem(item, ref weapons, ref weaponsDictionary);
    }

    public void AddItem(ItemData item, ref List<InventoryItem> itemList, ref Dictionary<ItemData, InventoryItem> itemDictionary)
    {
        if (itemDictionary.TryGetValue(item, out InventoryItem value))
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

        newEquipment.isEquipped = true;

        if (newEquipment.subEquipmentType == EquipmentType.Weapon)
        {
            EquipWeapon(item, itemSlot);
            return;
        }

        newEquipment.AddModifiers();
        if (newEquipment.subEquipmentType == EquipmentType.Defence)
        {
            defenceParts.Add(newItem);
            defencePartsDictionary[newEquipment] = newItem;
        }

        if (itemSlot.item != null && itemSlot.item.data != null)
        {
            UnequipItem(itemSlot);
        }

        itemSlot.item = newItem;
        itemSlot.UpdateSlot(newItem);
    }

    public void EquipWeapon(ItemData item, UI_ItemSlot itemSlot)
    {
        PlayerWeaponController controller = PlayerManager.instance.player.weaponController;
        InventoryItem newItem = new InventoryItem(item as ItemData_Equipment);
        WeaponData weaponData = item as WeaponData;
        if (itemSlot == weaponsSlots[0])
        {
            controller.EquipWeapon(weaponData, 0);
        }
        else
        {
            controller.EquipWeapon(weaponData, 1);
        }

        itemSlot.item = newItem;
        itemSlot.UpdateSlot(newItem);
    }

    public void UnequipItem(UI_ItemSlot itemSlot)
    {
        if (itemSlot == null)
        {
            Debug.LogWarning("Empty Slot");
            return;
        }

        ItemData_Equipment item = itemSlot.item.data as ItemData_Equipment;
        UnequipItem(item);
        itemSlot.CleanUpSlot();
    }

    private void UnequipItem(ItemData_Equipment item)
    {
        if (item == null)
        {
            Debug.LogWarning("ItemData_Equipment not found!");
            return;
        }

        item.isEquipped = false;
        item.RemoveModifiers();

        UpdateEquipmentUI();
        UpdateSelectionSlotUI(item.subEquipmentType);
    }
    #endregion

    #region Remove Item From Inventory
    public void RemoveItem(ItemData item)
    {
        if (materialsDictionary.TryGetValue(item, out InventoryItem value))
        {
            RemoveItem(value, ref materials, ref materialsDictionary);
            //return;
        }

        ItemData_Equipment equipment = item as ItemData_Equipment;
        if (equipment != null)
        {
            if (amuletsDictionary.TryGetValue(equipment, out value))
                RemoveItem(value, ref amulets, ref amuletsDictionary);

            if (defencePartsDictionary.TryGetValue(equipment, out value))
                RemoveItem(value, ref defenceParts, ref defencePartsDictionary);

            UpdateEquipmentUI();
            UpdateSelectionSlotUI(equipment.subEquipmentType);
        }

        ItemData_Usable usableItem = item as ItemData_Usable;
        if (usableItem != null)
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

    public void UpdateEquipmentUI()
    {
        for (int i = 0; i < amuletSlots.Length; i++)
        {
            if (amuletSlots[i].item == null || amuletSlots[i].item.data == null)
                continue;

            ItemData_Equipment equipment = amuletSlots[i].item.data as ItemData_Equipment;
            if (amuletsDictionary.ContainsKey(equipment))
            {
                if (!equipment.isEquipped)
                    amuletSlots[i].CleanUpSlot();
            }
            else
                amuletSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < defencePartsSlots.Length; i++)
        {
            if(defencePartsSlots[i].item == null || defencePartsSlots[i].item.data == null)
                continue;

            ItemData_Equipment equipment = defencePartsSlots[i].item.data as ItemData_Equipment;
            if (defencePartsDictionary.ContainsKey(defencePartsSlots[i].item.data as ItemData_Equipment))
            {
                if (!equipment.isEquipped)
                    defencePartsSlots[i].CleanUpSlot();
            }
            else
                defencePartsSlots[i].CleanUpSlot();
        }
    }

    public void UpdateSelectionSlotUI(EquipmentType equipmentType)
    {
        for (int i = 0; i < selectionSlots.Length; i++)
            selectionSlots[i].CleanUpSlot();

        if (equipmentType == EquipmentType.Weapon)
        {
            for (int i = 0; i < weapons.Count; i++)
                selectionSlots[i].UpdateSlot(weapons[i]);
        }

        if (equipmentType != EquipmentType.Amulet)
        {
            int ctr = 0;
            for (int i = 0; i < defenceParts.Count; i++)
            {
                ItemData_Equipment key = defenceParts[i].data as ItemData_Equipment;
                if (key.subEquipmentType == equipmentType)
                    selectionSlots[ctr++].UpdateSlot(defenceParts[i]);
            }
        }
        else if (equipmentType == EquipmentType.Amulet)
        {
            for (int i = 0; i < amulets.Count; i++)
                selectionSlots[i].UpdateSlot(amulets[i]);
        }
    }

    private void CleanSlots()
    {
        for (int i = 0; i < amuletSlots.Length; i++)
            amuletSlots[i].CleanUpSlot();

        for (int i = 0; i < defencePartsSlots.Length; i++)
            defencePartsSlots[i].CleanUpSlot();
    }

    #endregion

    public void UpdateSelectedSlot(UI_ItemSlot selectedSlot) => this.selectedSlot = selectedSlot;
}
