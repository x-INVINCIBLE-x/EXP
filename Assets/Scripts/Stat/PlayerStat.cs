using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStats
{
    public Dictionary<Stats,  Stat> statDictionary;

    private void Start()
    {
        InitializeStatDictionary();
    }

    private void InitializeStatDictionary()
    {
        statDictionary = new Dictionary<Stats,  Stat>
        {
            { Stats.Vitality, vitality }, 
            { Stats.Vigor, vigor }, 
            { Stats.Capacity, capacity }, 
            { Stats.Motivity, motivity }, 
            { Stats.Technique, technique }, 
            { Stats.Advance,  advance }, 
            { Stats.Health,  health }, 
            { Stats.Stamina,  stamina }, 
            { Stats.StaminaRegain, staminaRegain }, 
            { Stats.Legion, legion }, 
            { Stats.FableSlot, fableSlot }, 
            { Stats.GuardRegain, guardRegain }, 
            { Stats.PhysicalAtk, physicalAtk }, 
            { Stats.FireAtk, fireAtk }, 
            { Stats.ElectricAtk, electricAtk }, 
            { Stats.AcidAtk, acidAtk }, 
            { Stats.PhysicalDef, physicalDef }, 
            { Stats.FireDef, fireDef }, 
            { Stats.ElectricDef, electricDef }, 
            { Stats.AcidDef, acidDef }, 
            { Stats.FireRes, fireRes }, 
            { Stats.ElectricRes, electricRes }, 
            { Stats.AcidRes, acidRes }, 
            { Stats.DisruptionRes, disruptionRes }, 
            { Stats.ShockRes, ShockRes }, 
            { Stats.BreakRes, breakRes }
        };
    }
}
