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
    StaminaRegain,
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
    FireRes,
    ElectricRes,
    AcidRes,
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
    public Stat staminaRegain;
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

    public float ailmentLimitOffset = 10;

    public float currentHealth;
    public float currentStamina;
    public float currentFableSlot;

    public bool isInvincible { get; private set; } = false;
    public bool isBlocking { get; private set; } = false;
    public bool isPerfectBlock { get; private set; } = false;
    public bool isConsumingStamina { get; private set; } = false;

    protected Dictionary<AilmentType, Action> ailmentActions;
    public Dictionary<Stats, Stat> statDictionary;

    public event Action UpdateHUD;

    [System.Serializable]
    public class AilmentStatus
    {
        public float Value;
        public Stat resistance;
        public Stat defence;
        public bool isMaxed = false;
        public float ailmentLimit = 100;
        public event Action<AilmentStatus> ailmentEffectEnded;
        public IEnumerator ReduceValueOverTime()
        {
            while (Value > 0)
            {
                if (Value > 0)
                {
                    Value -= resistance.Value * Time.deltaTime;
                    
                    if (Value <= 0)
                    {
                        if (isMaxed)
                            ailmentEffectEnded?.Invoke(this);

                        Value = 0;
                        isMaxed = false;
                    }

                    yield return new WaitForEndOfFrame();
                }
                yield return null;
            }
        }
    }

    protected virtual void Awake()
    {
        InitializeValues();

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

    private void Start()
    {
        InitializeStatDictionary();
    }

    private void InitializeValues()
    {
        currentHealth = health.Value;
        currentStamina = stamina.Value;
        currentFableSlot = fableSlot.Value;
    }

    public void InitializeStatDictionary()
    {
        statDictionary = new Dictionary<Stats, Stat>
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

    private void Update()
    {
        if (!isConsumingStamina && currentStamina < stamina.Value)
        {
            currentStamina += staminaRegain.Value * Time.deltaTime;
            UpdateHUD?.Invoke();
        }
    }

    public virtual void DoDamage(CharacterStats targetStats)
    {
        if (targetStats.isPerfectBlock)
        {
            Debug.Log("Perfect Block Successful!");
            return;
        }

        targetStats.TakePhysicalDamage(physicalAtk.Value);

        DoAilmentDamage(targetStats);
        targetStats.UpdateHUD?.Invoke();
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
            targetStats.TryApplyAilmentEffect(_fireAtk, ref targetStats.fireStatus, AilmentType.Fire);
        else if (_electricAtk > 0)
            targetStats.TryApplyAilmentEffect(_electricAtk, ref targetStats.electricStatus, AilmentType.Electric);
        else if (_acidAtk > 0)
            targetStats.TryApplyAilmentEffect(_acidAtk, ref targetStats.acidStatus, AilmentType.Acid);
        else if (_disruptionAtk > 0)
            targetStats.TryApplyAilmentEffect(_disruptionAtk, ref targetStats.disruptionStatus, AilmentType.Disruption);
        else if(_shockAtk > 0)
            targetStats.TryApplyAilmentEffect(_shockAtk , ref targetStats.shockStatus, AilmentType.Shock);
        else if(_breakAtk > 0)
            targetStats.TryApplyAilmentEffect(_breakAtk, ref targetStats.breakStatus, AilmentType.Break);
    }

    protected virtual void TryApplyAilmentEffect(float ailmentAtk, ref AilmentStatus ailmentStatus, AilmentType ailmentType)
    {
        if (ailmentStatus.isMaxed)
            return;

        float ailmentDefence = ailmentStatus.defence.Value;
        float effectAmount = ailmentAtk - ailmentDefence;
        ReduceHealthBy(effectAmount);

        ailmentStatus.Value = Mathf.Min(ailmentStatus.ailmentLimit + ailmentLimitOffset, ailmentStatus.Value + effectAmount);
        StartCoroutine(ailmentStatus.ReduceValueOverTime());

        UI.instance.ailmentSlider[((int)ailmentType)].gameObject.SetActive(true);
        StartCoroutine(UI.instance.ailmentSlider[((int)ailmentType)].UpdateUI());

        if (ailmentStatus.Value < ailmentStatus.ailmentLimit)
            return;

        ApplyAilment(ailmentType);
        ailmentStatus.isMaxed = true;
        ailmentStatus.ailmentEffectEnded += AilmentEffectEnded;
    }

    private void ApplyAilment(AilmentType ailmentType)
    {
        if (ailmentActions.TryGetValue(ailmentType,out var ailmentEffect))
            ailmentEffect();
    }

    protected virtual void AilmentEffectEnded(AilmentStatus ailmentStatus)
    {
        ailmentStatus.ailmentEffectEnded -= AilmentEffectEnded;
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
    
    public void SetConsumingStamina(bool status) => isConsumingStamina = status;

    public bool HasEnoughStamina(float staminaAmount)
    {
        if(currentStamina > staminaAmount)
        {
            currentStamina -= staminaAmount;
            UpdateHUD?.Invoke();
            return true;
        }

        return false;
    }
    
    public bool HasEnoughFableSlot(int slots)
    {
        if(currentFableSlot > (slots * 100))
        {
            currentFableSlot -= (slots * 100);
            UpdateHUD?.Invoke();
            return true;
        }
        return false;
    }
    
    public void ChargeFable(int amount)
    {
        if (currentFableSlot >= fableSlot.Value)
            return;

        currentFableSlot = Mathf.Min(currentFableSlot + amount,fableSlot.Value);
    }
   
    public float GetCurrentStamina() => currentStamina;
}