using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stats
{
    Vitality,
    Vigor,
    Capacity,
    Motivity,
    Technique,
    Advance,
    Health,
    Stamina,
    Legion,
    FableSlot,
    GuardRegain,
    PhysicalAtkLight,
    PhysicalAtkHeavy,
    FireAtk,
    ElectricAtk,
    AcidAtk,
    PhysicalDef,
    FireDef,
    ElectricDef,
    AcidDef,
    DisruptionRes,
    ShockRes,
    BreakRes
}

public class CharacterStats : MonoBehaviour
{
    [Header("Default Abilities")]
    public Stat vitality;
    public Stat vigor;
    public Stat capacity;
    public Stat motivity;
    public Stat technique;
    public Stat advance;

    [Header("Common Abilities")]
    public Stat health;
    public Stat stamina;
    public Stat legion;
    public Stat fableSlot;
    public Stat guardRegain;

    [Header("Attack Abilities")]
    public Stat physicalAtkLight;
    public Stat physicalAtkheavy;
    public Stat fireAtk;
    public Stat electricAtk;
    public Stat acidAtk;

    [Header("Defence")]
    public Stat physicalDef;
    public Stat fireDef;
    public Stat electricDef;
    public Stat acidDef;

    [Space]
    public Stat disruptionRes;
    public Stat ShockRes;
    public Stat breakRes;

    public int currentHealth;

    public void DoDamage(CharacterStats targetStats)
    {

    }

    public void TakeDamage()
    {

    }
}
