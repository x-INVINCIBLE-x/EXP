using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> usableItems = new();
    public Dictionary<ItemData, InventoryItem> usableItemsDictionary = new();

    public List<InventoryItem> materials = new();
    public Dictionary<ItemData, InventoryItem> materialsDictionary = new();

    public List<InventoryItem> weapons = new();

    public List<InventoryItem> defenceParts = new();
    public Dictionary<ItemData_Equipment, InventoryItem> defncePartsDictionary = new();

    public List<InventoryItem> amulets = new();
    public Dictionary<ItemData_Equipment, InventoryItem> amuletsDictionary = new();

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
            AddItem(item, ref usableItems, ref defncePartsDictionary);
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

    //public void EquipItem(ItemData item)
    //{
    //    ItemData_Equipment newEquipment = item as ItemData_Equipment;
    //    InventoryItem newItem = new InventoryItem(newEquipment);
    //}

    public void RemoveItem(ItemData item)
    {
        if (materialsDictionary.TryGetValue(item, out InventoryItem value))
            RemoveItem(value, ref materials, ref materialsDictionary);
        else if (usableItemsDictionary.TryGetValue(item, out value))
            RemoveItem(value, ref usableItems, ref usableItemsDictionary);
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
    }
}
