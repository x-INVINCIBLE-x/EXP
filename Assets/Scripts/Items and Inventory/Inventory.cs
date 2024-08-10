using System.Collections.Generic;
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

    //public List<InventoryItem> upperBelt = new();
    //public List<InventoryItem> lowerBelt = new();
    //public List<InventoryItem> extraBag = new();

    public List<ItemData> startingItems;

    [Header("Parent Slots")]
    public Transform defencePartsParent;
    public Transform amuletsParent;
    public Transform weaponsParent;
    public Transform upperBeltParent;
    public Transform lowerBeltParent;
    public Transform extraBagParent;

    public Transform activeUpperBeltParent;
    public Transform activeLowerBeltParent;

    public Transform selectionSlotsParent;

    [Header("Slots Info")]
    [SerializeField] private UI_EquipmentSlot[] defencePartsSlots;
    [SerializeField] private UI_EquipmentSlot[] amuletSlots;
    [SerializeField] private UI_EquipmentSlot[] weaponsSlots;
    [SerializeField] private UI_BeltSlot[] upperBeltSlots;
    [SerializeField] private UI_BeltSlot[] lowerBeltSlots;
    [SerializeField] private UI_BeltSlot[] extraBeltSlots;

    [SerializeField] private UI_ActiveBeltSlot[] activeUpperBeltSlots;
    [SerializeField] private UI_ActiveBeltSlot[] activeLowerBeltSlots;

    [SerializeField] private UI_SelectionSlot[] selectionSlots;

    [Header("Bag Panel Parents")]
    public Transform bagSlotsParent;

    [Header("Bag Panel Slots")]
    [SerializeField][HideInInspector] private UI_BagSlots[] bagSlots;

    [SerializeField] private UI_ItemSlot selectedSlot;
    private BeltType activeBeltSelected = BeltType.UpperBelt;

    private InputManager inputManager;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        selectionSlots = selectionSlotsParent.GetComponentsInChildren<UI_SelectionSlot>(true);

        defencePartsSlots = defencePartsParent.GetComponentsInChildren<UI_EquipmentSlot>();
        amuletSlots = amuletsParent.GetComponentsInChildren<UI_EquipmentSlot>();
        weaponsSlots = weaponsParent.GetComponentsInChildren<UI_EquipmentSlot>();
        lowerBeltSlots = lowerBeltParent.GetComponentsInChildren<UI_BeltSlot>();
        upperBeltSlots = upperBeltParent.GetComponentsInChildren<UI_BeltSlot>();
        extraBeltSlots = extraBagParent.GetComponentsInChildren<UI_BeltSlot>();

        activeUpperBeltSlots = activeUpperBeltParent.GetComponentsInChildren<UI_ActiveBeltSlot>();
        activeLowerBeltSlots = activeLowerBeltParent.GetComponentsInChildren<UI_ActiveBeltSlot>();

        bagSlots = bagSlotsParent.GetComponentsInChildren<UI_BagSlots>(true);
    }

    private void Start()
    {
        AddStartingItems();
        ShowBagItemSlots(ItemType.UsableItem);
        CleanSlots();
        UpdateActiveBeltUI();
        UpdateBeltUI();
    }

    private void OnEnable()
    {
        inputManager = InputManager.Instance;
        inputManager.UpdateUpperBeltEvent += ShiftUpperBelt;
        inputManager.UpdateLowerBeltEvent += ShiftLowerBelt;
        inputManager.UseEvent += UseBeltItem;
        inputManager.On1Event += UseExtraBagItem;
        inputManager.On2Event += UseExtraBagItem;
        inputManager.On3Event += UseExtraBagItem;
        inputManager.On4Event += UseExtraBagItem;
    }

    private void OnDisable()
    {
        inputManager.UpdateUpperBeltEvent -= ShiftUpperBelt;
        inputManager.UpdateLowerBeltEvent -= ShiftLowerBelt;
        inputManager.UseEvent -= UseBeltItem;
        inputManager.On1Event -= UseExtraBagItem;
        inputManager.On2Event -= UseExtraBagItem;
        inputManager.On3Event -= UseExtraBagItem;
        inputManager.On4Event -= UseExtraBagItem;
    }

    //Temporary Input Check
    private void Update()
    {
        if (!UI.instance.hasActivePanels())
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (selectedSlot == null || selectedSlot.item == null || selectedSlot.item.data == null)
                return;

            ItemData_Equipment equipment = selectedSlot.item.data as ItemData_Equipment;
            if (equipment && equipment.isEquipped)
            {
                UnequipItem(equipment);
                return;
            }

            ItemData_Usable usableItem = selectedSlot.item.data as ItemData_Usable;
            if (usableItem && usableItem.isEquipped)
            {
                UnequipItem(usableItem);
                return;
            }

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
    public void EquipItem(InventoryItem item, UI_ItemSlot itemSlot)
    {
        ItemData_Equipment newEquipment = item.data as ItemData_Equipment;

        if (itemSlot.item != null && itemSlot.item.data != null)
            UnequipItem(itemSlot);

        if (newEquipment && newEquipment.subEquipmentType == EquipmentType.Weapon)
        {
            EquipWeapon(itemSlot, item.data, item);
            return;
        }

        ItemData_Usable usableItem = item.data as ItemData_Usable;
        if (usableItem)
        {
            EquipUsableItem(itemSlot, item.data, item);
            return;
        }

        EquipEquipment(itemSlot, newEquipment, item);
    }

    private void EquipEquipment(UI_ItemSlot itemSlot, ItemData_Equipment newEquipment, InventoryItem newItem)
    {
        newEquipment.AddModifiers();
        newEquipment.isEquipped = true;

        if (newEquipment.subEquipmentType == EquipmentType.Defence)
        {
            defenceParts.Add(newItem);
            defencePartsDictionary[newEquipment] = newItem;
        }

        itemSlot.item = newItem;
        itemSlot.UpdateSlot(newItem);
    }

    private void EquipWeapon(UI_ItemSlot itemSlot, ItemData item, InventoryItem newItem)
    {
        PlayerWeaponController controller = PlayerManager.instance.player.weaponController;
        WeaponData weaponData = item as WeaponData;

        if (weaponData.isEquipped)
            return;

        if (itemSlot == weaponsSlots[0])
        {
            controller.EquipWeapon(weaponData, 0);
        }
        else
        {
            controller.EquipWeapon(weaponData, 1);
        }

        weaponData.isEquipped = true;
        itemSlot.item = newItem;
        itemSlot.UpdateSlot(newItem);
    }

    public void EquipUsableItem(UI_ItemSlot itemSlot, ItemData item, InventoryItem newItem)
    {
        UI_BeltSlot beltSlot = itemSlot as UI_BeltSlot;
        ItemData_Usable usableItem = item as ItemData_Usable;

        if (!beltSlot || !usableItem)
        {
            Debug.LogWarning("Usable Belt or Item NUll");
            return;
        }

        usableItem.isEquipped = true;

        //if (beltSlot.type == BeltType.LowerBelt)
        //    lowerBelt.Add(newItem);
        //else if (beltSlot.type == BeltType.UpperBelt)
        //    upperBelt.Add(newItem);
        //else if (beltSlot.type == BeltType.Extra)
        //    extraBag.Add(newItem);

        itemSlot.item = newItem;
        itemSlot.UpdateSlot(newItem);

        UpdateActiveBeltUI();
    }

    public void UnequipItem(UI_ItemSlot itemSlot)
    {
        if (itemSlot == null)
        {
            Debug.LogWarning("Empty Slot");
            return;
        }

        itemSlot.item.data.isEquipped = false;
        
        ItemData_Equipment equipment = itemSlot.item.data as ItemData_Equipment;
        if (equipment)
            UnequipItem(equipment);

        itemSlot.CleanUpSlot();
    }

    private void UnequipItem(ItemData_Equipment item)
    {
        if (item == null)
        {
            Debug.LogWarning("ItemData_Equipment not found!");
            return;
        }

        WeaponData weapondata = item as WeaponData;
        if (weapondata)
            PlayerManager.instance.player.weaponController.UnequipWeapon(weapondata);

        item.isEquipped = false;
        item.RemoveModifiers();

        UpdateEquipmentUI();
        UpdateSelectionSlotUI(item.subEquipmentType);
    }

    private void UnequipItem(ItemData_Usable usableItem)
    {
        usableItem.isEquipped = false;

        UpdateBeltUI();
        UpdateSelectionSlotUI(usableItem.itemType);
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
        UpdateEquipmentSlots(amuletSlots, amuletsDictionary);
        UpdateEquipmentSlots(defencePartsSlots, defencePartsDictionary);
        UpdateEquipmentSlots(weaponsSlots, weaponsDictionary);
    }

    private void UpdateEquipmentSlots(UI_EquipmentSlot[] equipmentSlots, Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].item == null || equipmentSlots[i].item.data == null)
                continue;

            ItemData_Equipment equipment = equipmentSlots[i].item.data as ItemData_Equipment;
            if (equipmentDictionary.ContainsKey(equipment))
            {
                if (!equipment.isEquipped)
                    equipmentSlots[i].CleanUpSlot();
            }
            else
                equipmentSlots[i].CleanUpSlot();
        }
    }

    public void UpdateBeltUI()
    {
        UpdateBeltSlots(upperBeltSlots);
        UpdateBeltSlots(lowerBeltSlots);
        UpdateBeltSlots(extraBeltSlots);        
    }

    private void UpdateBeltSlots(UI_BeltSlot[] beltSlots)
    {
        for (int i = 0; i < beltSlots.Length; i++)
        {
            if (beltSlots[i].item == null || beltSlots[i].item.data == null)
            {
                beltSlots[i].CleanUpSlot(); 
                continue;
            }

            ItemData_Usable usableItem = beltSlots[i].item.data as ItemData_Usable;

             //If unequipped item is in use
            if (!usableItemsDictionary.ContainsKey(usableItem) || !usableItem.isEquipped)
            {
                beltSlots[i].CleanUpSlot();
                UpdateActiveBeltUI();
            }
            else
                beltSlots[i].UpdateSlot(beltSlots[i].item);
        }
    }

    // For Equipment
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

    // For Usable Item
    public void UpdateSelectionSlotUI(ItemType itemType)
    {
        for (int i = 0; i < selectionSlots.Length; i++)
            selectionSlots[i].CleanUpSlot();

        if (itemType == ItemType.UsableItem)
        {
            for (int i = 0; i < usableItems.Count; i++)
            {
                selectionSlots[i].UpdateSlot(usableItems[i]);
            }
        }
    }

    public void UpdateActiveBeltUI()
    {
        for (int i = 0; i < activeUpperBeltSlots.Length; i++)
            activeUpperBeltSlots[i].UpdateSlot(upperBeltSlots[i].item);

        for (int i = 0; i < activeLowerBeltSlots.Length; i++)
            activeLowerBeltSlots[i].UpdateSlot(lowerBeltSlots[i].item);
    }


    private void CleanSlots()
    {
        for (int i = 0; i < amuletSlots.Length; i++)
            amuletSlots[i].CleanUpSlot();

        for (int i = 0; i < defencePartsSlots.Length; i++)
            defencePartsSlots[i].CleanUpSlot();

        for (int i = 0; i < upperBeltSlots.Length; i++)
            upperBeltSlots[i].CleanUpSlot();

        for (int i = 0; i < activeUpperBeltSlots.Length; i++)
            activeUpperBeltSlots[i].CleanUpSlot();
    }

    #endregion

    public void ShiftBeltSlots(BeltType beltType)
    {
        if (beltType == BeltType.UpperBelt)
            ShiftSlots(activeUpperBeltSlots);
        else if (beltType == BeltType.LowerBelt)
            ShiftSlots(activeLowerBeltSlots);
    }

    private void ShiftSlots(UI_ActiveBeltSlot[] activeBeltSlots)
    {
        InventoryItem startItem = activeBeltSlots[0].item;
        int activeBeltSlotsLength = activeBeltSlots.Length;

        for (int i = 1; i < activeBeltSlotsLength; i++)
        {
            InventoryItem nextItem = activeBeltSlots[i].item;
            activeBeltSlots[i - 1].UpdateSlot(activeBeltSlots[i].item);
        }

        activeBeltSlots[activeBeltSlotsLength - 1].UpdateSlot(startItem);
    }

    public void ShiftUpperBelt()
    {
        if (activeBeltSelected == BeltType.LowerBelt)
        {
            activeBeltSelected = BeltType.UpperBelt;
            UI.instance.ActivateSlotVisualizer(BeltType.UpperBelt);
            return;
        }

        ShiftBeltSlots(BeltType.UpperBelt);
    }

    public void ShiftLowerBelt()
    {
        if (activeBeltSelected == BeltType.UpperBelt)
        {
            activeBeltSelected = BeltType.LowerBelt;
            UI.instance.ActivateSlotVisualizer(BeltType.LowerBelt);
            return;
        }

        ShiftBeltSlots(BeltType.LowerBelt);
    }

    public void UseBeltItem()
    {
        UI_ActiveBeltSlot activeSlot;
        if (activeBeltSelected == BeltType.UpperBelt)
            activeSlot = activeUpperBeltSlots[0];
        else
            activeSlot = activeLowerBeltSlots[0];

        if (activeSlot.item == null || activeSlot.item.data == null)
            return;

        UseItem(activeSlot.item.data);
    }

    public void UseExtraBagItem(int index)
    {
        if (extraBeltSlots[index].item == null || extraBeltSlots[index].item.data == null)
            return;

        UseItem(extraBeltSlots[index].item.data);

    }

    public void UseItem(ItemData item)
    {
        ItemData_Usable usableItem = item as ItemData_Usable;

        if(usableItem == null)
        {
            Debug.LogWarning("Trying to use non usable item");
            return;
        }

        RemoveItem(item);
        UpdateBeltUI();

        if (activeBeltSelected == BeltType.LowerBelt)
            activeLowerBeltSlots[0].UpdateSlot(activeLowerBeltSlots[0].item);
        else
            activeUpperBeltSlots[0].UpdateSlot(activeUpperBeltSlots[0].item);

        usableItem.UseItem(PlayerManager.instance.player.stats);
    }

    public void UpdateSelectedSlot(UI_ItemSlot selectedSlot) => this.selectedSlot = selectedSlot;
}
