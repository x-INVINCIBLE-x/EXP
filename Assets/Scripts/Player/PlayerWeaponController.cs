using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;
    private PlayerStat stats;

    public WeaponData currentWeapon;
    public WeaponData backupWeapon;
    private GameObject currentWeaponModel;

    private int lightAttackIndex = -1;
    private int heavyAttackindex = -1;
    private Attack lastAttack;
    private float lastTimeAttacked;

    [Header("Multi Fable Attack")]
    public float lastMultiAttackCalled = 0f;
    public float animTime = -10f;
    public float bufferTime = 1f;
    public int lastIndex = -1;

    [Header("Buff Fable Art")]
    public float buffActivateCooldown;
    private float lastBuffCalled;

    private int currentWeaponIndex = 0;

    private void Start()
    {
        stats = PlayerManager.instance.player.stats;
        InitializeWeapons();
        AddModifiersOfCurrentWeapon();
    }

    private void OnEnable()
    {
    }

    public void InitializeWeapons()
    {
        List<InventoryItem> weapons = Inventory.Instance.weapons;
        currentWeapon = null;
        backupWeapon = null;

        if(currentWeapon)
            currentWeaponModel = Instantiate(currentWeapon.weaponDetails.model, weaponHolder);
    }

    public void EquipWeapon(WeaponData weaponData, int index)
    {
        // 0 -> current Weapon
        // 1 -> backup weapon

        if (index == currentWeaponIndex)
        {
            if (currentWeapon)
            {
                Destroy(currentWeaponModel);
                RemoveModifiersFromCurretWeapon();
            }

            currentWeapon = weaponData;
            AddModifiersOfCurrentWeapon();
            CreateWeaponModel();
        }
        else
            backupWeapon = weaponData;
    }

    public void UnequipWeapon(WeaponData weaponData)
    {
        if (currentWeapon == weaponData)
        {
            currentWeapon = null;
            Destroy(currentWeaponModel);
        }
        else if (weaponData == backupWeapon)
            backupWeapon = null;
    }

    public Attack SelectLightAttack()
    {
        if (currentWeapon == null)
            return null;

        if (!CanCombo() && HasAttackStamina(currentWeapon.heavyAttack[0]))
            return currentWeapon.lightAttack[0];

        lightAttackIndex = lightAttackIndex + 1 >= currentWeapon.lightAttack.Length? 0 : lightAttackIndex + 1;

        if(heavyAttackindex > 0)
        {
            ResetIndexes();
            lightAttackIndex = 0;
        }

        lastTimeAttacked = Time.time;
        Attack attack = currentWeapon.lightAttack[lightAttackIndex];

        if (!HasAttackStamina(attack))
        {
            if (!HasAttackStamina(currentWeapon.lightAttack[0]))
                return null;

            attack = currentWeapon.lightAttack[0];
        }

        lastAttack = attack;
        return attack;
    }

    public Attack SelectHeavyAttack()
    {
        if (currentWeapon == null)
            return null;

        if (!CanCombo() && HasAttackStamina(currentWeapon.heavyAttack[0]))
            return currentWeapon.heavyAttack[0];

        heavyAttackindex = heavyAttackindex + 1 >= currentWeapon.heavyAttack.Length ? 0 : heavyAttackindex + 1; 

        if (lightAttackIndex > 0)
        {
            ResetIndexes();
            heavyAttackindex = 0;
        }
        
        lastTimeAttacked = Time.time;
        Attack attack = currentWeapon.heavyAttack[heavyAttackindex];

        if (!HasAttackStamina(attack))
        {
            if (!HasAttackStamina(currentWeapon.heavyAttack[0]))
                return null;

            attack = currentWeapon.heavyAttack[0];
        }

        lastAttack = attack;
        return attack;
    }

    public Attack SelectChargeAttack()
    {
        if (currentWeapon == null)
            return null;

        ResetIndexes();
        Attack attack = currentWeapon.chargeAttack;

        if (!HasAttackStamina(attack))
        {
            return null;
        }

        lastAttack = null;
        return attack;
    }

    public bool CanCombo()
    {
        if (lastAttack == null)
            return true;

        if(Time.time < lastTimeAttacked + lastAttack.clip.length + lastAttack.comboTime)
            return true;

        lastAttack = null;
        ResetIndexes();
        return false;
    }

    public bool HasAttackStamina(Attack attack)
    {
        if (PlayerManager.instance.player.stats.HasEnoughStamina(attack.staminaConsumption))
        {
            return true;
        }

        return false;
    }

    private void ResetIndexes()
    {
        lightAttackIndex = -1;
        heavyAttackindex = -1;
    }

    public void SwitchWeapon()
    {
        Destroy(currentWeaponModel);

        RemoveModifiersFromCurretWeapon();

        (currentWeapon, backupWeapon) = (backupWeapon, currentWeapon);

        AddModifiersOfCurrentWeapon();

        currentWeaponIndex = currentWeaponIndex == 0 ? 1 : 0;
    }

    private void AddModifiersOfCurrentWeapon()
    {
        if (currentWeapon == null)
            return;

        stats.fireAtk.AddModifier(new StatModifier(currentWeapon.fireAtk, StatModType.Flat, currentWeapon));
        stats.electricAtk.AddModifier(new StatModifier(currentWeapon.electricAtk, StatModType.Flat, currentWeapon));
        stats.acidAtk.AddModifier(new StatModifier(currentWeapon.acidAtk, StatModType.Flat, currentWeapon));
    }

    private void RemoveModifiersFromCurretWeapon()
    {
        if (currentWeapon == null)
            return;

        stats.fireAtk.RemoveAllModifiersFromSource(currentWeapon);
        stats.electricAtk.RemoveAllModifiersFromSource(currentWeapon);
        stats.acidAtk.RemoveAllModifiersFromSource(currentWeapon);
    }

    public void CreateWeaponModel()
    {
        currentWeaponModel = Instantiate(currentWeapon.weaponDetails.model, weaponHolder);
    }

    public void ExecuteFableArt()
    {
        if (currentWeapon == null)
            return;

        if (currentWeapon.fableBlade.type == FableArtType.Buff)
        {
            if (Time.time < lastBuffCalled + buffActivateCooldown)
                return;

            FableArt_Buff buffFable = (FableArt_Buff)currentWeapon.fableBlade;
            buffActivateCooldown = buffFable.duration;
            lastBuffCalled = Time.time;
            currentWeapon.fableBlade.Execute();
            return;
        }


        if (currentWeapon.fableBlade.type == FableArtType.MultiAttack)
        {
            MultiFableAttack();
            return;
        }
            
        currentWeapon.fableBlade.Execute();
    }

    public void MultiFableAttack()
    {
        if (currentWeapon == null)
            return;

        FableArt_MultiAttack fableAttack = currentWeapon.fableBlade as FableArt_MultiAttack;
        
        if (Time.time < lastMultiAttackCalled + animTime)
            return;

        if (Time.time < lastMultiAttackCalled + animTime + bufferTime)
            lastMultiAttackCalled = Time.time;
        else
            lastIndex = -1;

        lastMultiAttackCalled = Time.time;

        int _index = lastIndex + 1 == fableAttack.attacks.Length ? 0 : lastIndex + 1;
        lastIndex = _index;
        animTime = fableAttack.attacks[_index].clip.length;
        fableAttack.Execute(_index);
    }
}