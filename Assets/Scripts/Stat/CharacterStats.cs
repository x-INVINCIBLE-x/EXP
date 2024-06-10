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
    PhysicalAtk,
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
public enum Ailment
{
    Fire,
    Electric,
    Acid,
    Disruption,
    Shock,
    Break
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
    public Stat physicalAtk;
    public Stat fireAtk;
    public Stat electricAtk;
    public Stat acidAtk;
    public Stat disruptionAtk;
    public Stat shockAtk;
    public Stat breakAtk;

    [Header("Defence")]
    public Stat physicalDef;
    public Stat fireDef;
    public Stat electricDef;
    public Stat acidDef;

    [Space]
    public Stat fireRes;
    public Stat electricRes;
    public Stat acidRes;
    public Stat disruptionRes;
    public Stat ShockRes;
    public Stat breakRes;

    [Header("Ailment Status")]
    public float fireStatus;
    public float electricStatus;
    public float acidStatus;
    public float disruptionStatus;
    public float shockStatus;
    public float breakStatus;

    public float ailmentLimit = 100;
    public float ailmentLimitOffset = 10;

    public float currentHealth;

    public void DoDamage(CharacterStats targetStats)
    {
        targetStats.TakeDamage(physicalAtk.Value);


    }

    public void DoAilmentDamage(CharacterStats targetStats)
    {
        float _fireAtk = fireAtk.Value;
        float _electricAtk = electricAtk.Value;
        float _acidAtk = acidAtk.Value;
        float _disruptionAtk = disruptionAtk.Value;
        float _shockAtk = shockAtk.Value;
        float _breakAtk = breakAtk.Value;

        float damage = _fireAtk + _electricAtk + _acidAtk + _disruptionAtk + _shockAtk + _breakAtk;

        if (damage == 0)
            return;

        if (_fireAtk > 0)
            targetStats.TryApplyAilmentEffect(_fireAtk, targetStats.fireRes.Value, ref targetStats.fireStatus, Ailment.Fire);
        else if (_electricAtk > 0)
            targetStats.TryApplyAilmentEffect(_electricAtk, targetStats.electricRes.Value, ref targetStats.electricStatus, Ailment.Electric);
        else if (_acidAtk > 0)
            targetStats.TryApplyAilmentEffect(_acidAtk, targetStats.acidRes.Value, ref targetStats.acidStatus, Ailment.Acid);
        else if (_disruptionAtk > 0)
            targetStats.TryApplyAilmentEffect(_disruptionAtk, targetStats.disruptionRes.Value, ref targetStats.disruptionStatus, Ailment.Disruption);
        else if(_shockAtk > 0)
            targetStats.TryApplyAilmentEffect(_shockAtk, targetStats.ShockRes.Value, ref targetStats.shockStatus, Ailment.Shock);
        else if(_breakAtk > 0)
            targetStats.TryApplyAilmentEffect(_breakAtk, targetStats.breakRes.Value, ref targetStats.breakStatus, Ailment.Break);

    }

    private void TryApplyAilmentEffect(float ailmentAtk, float ailmentDef, ref float ailmentStatus, Ailment ailmentType)
    {
        float effectAmount = ailmentAtk - ailmentDef;
        ailmentStatus = Mathf.Max(ailmentLimit + ailmentLimitOffset, ailmentStatus + effectAmount);

        if (ailmentStatus >= ailmentLimit)
        {
            ApplyAilment(ailmentType);   
        }
    }

    private void ApplyAilment(Ailment ailmentType)
    {

    }

    public void TakeDamage(float damage)
    {
        float reducedDamage = Mathf.Max(0, damage - physicalDef.Value);

        currentHealth = Mathf.Max(0f, currentHealth - reducedDamage);
    }
}
