using System;
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
    Linear,
    Weapon,
    None
}

[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    public EquipmentType subEquipmentType;

    public List<Modifier> modifiers;

    private PlayerStat playerStats;

    public void AddModifiers()
    {
        playerStats = PlayerManager.instance.player.stats;
        
        if (playerStats.statDictionary == null)
            playerStats.InitializeStatDictionary();

        foreach (Modifier modifier in modifiers)
        {
            if (playerStats.statDictionary.TryGetValue(modifier.stat, out Stat stat))
            {
                StatModifier statMod = new StatModifier(modifier.value, modifier.modType, this);
                stat.AddModifier(statMod);
            }
            else
            {
                Debug.LogWarning("Stat not found" + modifier.stat);
            }
        }
    }

    public void RemoveModifiers()
    {
        playerStats = PlayerManager.instance.player.stats;

        foreach (var stat in playerStats.statDictionary.Values)
        {
            stat.RemoveAllModifiersFromSource(this);
        }
    }
}
