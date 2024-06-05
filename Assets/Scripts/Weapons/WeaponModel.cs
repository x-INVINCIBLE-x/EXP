using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RangeType { Melee, Ranged };
public enum HoldType { Upper, Lower };
public enum EquipType { Belt , Back };

[System.Serializable]
public class WeaponModel 
{
    public RangeType rangeType;
    public HoldType holdType;
    public EquipType equipType;
}
