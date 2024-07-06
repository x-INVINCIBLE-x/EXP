using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Amulet,
    Defence,
    Frame,
    Converter,
    Cartridge,
    Linear
}

[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    public EquipmentType subEquipmentType;
}
