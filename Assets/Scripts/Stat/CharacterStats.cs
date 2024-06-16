using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
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
public enum AilmentType
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
    public AilmentStatus fireStatus;
    public AilmentStatus electricStatus;
    public AilmentStatus acidStatus;
    public AilmentStatus disruptionStatus;
    public AilmentStatus shockStatus;
    public AilmentStatus breakStatus;

    public float ailmentLimit = 100;
    public float ailmentLimitOffset = 10;

    public float currentHealth;

    public bool isInvincible { get; private set; } = false;
    public bool isBlocking { get; private set; } = false;
    public bool isPerfectBlock { get; private set; } = false;
    public bool hasBeenHit { get; private set; } = false;

    private Dictionary<AilmentType, Action> ailmentActions;

    [System.Serializable]
    public class AilmentStatus
    {
        public float Value = 0f;
        public bool isMaxed = false;
    }

    private void Awake()
    {
        currentHealth = health.Value;

        ailmentActions = new Dictionary<AilmentType, Action>
        {
            { AilmentType.Fire, ApplyFireAilment },
            { AilmentType.Electric, ApplyElectricAilment },
            { AilmentType.Acid, ApplyAcidAilment },
            { AilmentType.Disruption, ApplyDisruptionAilment },
            { AilmentType.Shock, ApplyShockAilment },
            { AilmentType.Break, ApplyBreakAilment }
        };
    }

    private void Update()
    {
        ReduceAilmentStatus();
    }

    private void ReduceAilmentStatus()
    {
        if (fireStatus.Value > 0f)
            fireStatus.Value -= fireRes.Value * Time.deltaTime;
        if (breakStatus.Value > 0f)
            breakStatus.Value -= breakRes.Value * Time.deltaTime;
        if (electricStatus.Value > 0f)
            electricStatus.Value -= electricRes.Value * Time.deltaTime;
        if (acidStatus.Value > 0f)
            acidStatus.Value -= acidRes.Value * Time.deltaTime;
        if (disruptionStatus.Value > 0f)
            disruptionStatus.Value -= disruptionRes.Value * Time.deltaTime;
        if (shockStatus.Value > 0f)
            shockStatus.Value -= ShockRes.Value * Time.deltaTime;
    }

    public void DoDamage(CharacterStats targetStats)
    {
        if(targetStats.isPerfectBlock)
            return;

        targetStats.TakePhysicalDamage(physicalAtk.Value);

        DoAilmentDamage(targetStats);
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
            targetStats.TryApplyAilmentEffect(_fireAtk, targetStats.fireRes.Value, ref targetStats.fireStatus, AilmentType.Fire);
        else if (_electricAtk > 0)
            targetStats.TryApplyAilmentEffect(_electricAtk, targetStats.electricRes.Value, ref targetStats.electricStatus, AilmentType.Electric);
        else if (_acidAtk > 0)
            targetStats.TryApplyAilmentEffect(_acidAtk, targetStats.acidRes.Value, ref targetStats.acidStatus, AilmentType.Acid);
        else if (_disruptionAtk > 0)
            targetStats.TryApplyAilmentEffect(_disruptionAtk, targetStats.disruptionRes.Value, ref targetStats.disruptionStatus, AilmentType.Disruption);
        else if(_shockAtk > 0)
            targetStats.TryApplyAilmentEffect(_shockAtk, targetStats.ShockRes.Value, ref targetStats.shockStatus, AilmentType.Shock);
        else if(_breakAtk > 0)
            targetStats.TryApplyAilmentEffect(_breakAtk, targetStats.breakRes.Value, ref targetStats.breakStatus, AilmentType.Break);

    }

    private void TryApplyAilmentEffect(float ailmentAtk, float ailmentDef, ref AilmentStatus ailmentStatus, AilmentType ailmentType)
    {
        if (ailmentStatus.isMaxed)
            return;

        float effectAmount = ailmentAtk - ailmentDef;
        ReduceHealthBy(effectAmount);
        ailmentStatus.Value = Mathf.Min(ailmentLimit + ailmentLimitOffset, ailmentStatus.Value + effectAmount);

        if (ailmentStatus.Value >= ailmentLimit)
        {
            ApplyAilment(ailmentType);
            SetMax(ailmentStatus, ailmentDef);
        }
    }

    private void ApplyAilment(AilmentType ailmentType)
    {
        if (ailmentActions.TryGetValue(ailmentType,out var ailmentEffect))
            ailmentEffect();
    }

    #region Ailment Specific functions

    private void ApplyFireAilment()
    {

    }

    private void ApplyElectricAilment()
    {

    }

    private void ApplyAcidAilment()
    {

    }

    private void ApplyDisruptionAilment()
    {

    }

    private void ApplyShockAilment()
    {

    }

    private void ApplyBreakAilment()
    {

    }

    #endregion

    private void SetMax(AilmentStatus ailmentStatus, float def)
    {
        float time = ailmentLimit / def;
        StartCoroutine(StartSetMax(ailmentStatus, time));
    }

    private IEnumerator StartSetMax(AilmentStatus ailmentStatus, float time)
    {
        ailmentStatus.isMaxed = true;

        yield return new WaitForSeconds(time);

        ailmentStatus.Value = 0f;
        ailmentStatus.isMaxed = false;
    }

    public void TakePhysicalDamage(float damage)
    {
        float reducedDamage = Mathf.Max(0, damage - physicalDef.Value);

        ReduceHealthBy(reducedDamage);
    }

    public void ReduceHealthBy(float damage)
    {
        if (isInvincible)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - damage);
    }

    public void SetInvincibleFor(float time) => StartCoroutine(MakeInvincibleFor(time));

    private IEnumerator MakeInvincibleFor(float time)
    {
        isInvincible = true;

        yield return new WaitForSeconds(time);

        isInvincible = false;
    }

    public void SetInvincible(bool invincible) => isInvincible = invincible;

    public void SetBlocking(bool blocking) => isBlocking = blocking;
    
    public void SetPerfectBlock(bool perfectBlock) => isPerfectBlock = perfectBlock;

    public void ResetBool() => hasBeenHit = false;
    public void ResetBoolAfterDelay() => StartCoroutine(ResetHitBool());
    public IEnumerator ResetHitBool()
    {
        hasBeenHit = true;

        yield return new WaitForSeconds(0.1f);

        hasBeenHit = false;
    }
}
