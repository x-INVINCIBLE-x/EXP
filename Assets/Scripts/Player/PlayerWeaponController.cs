using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;

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

    private void Start()
    {
        currentWeaponModel = Instantiate(currentWeapon.weaponDetails.model, weaponHolder);
    }

    public Attack SelectLightAttack()
    {
        if (!CanCombo())
            return currentWeapon.lightAttack[0];

        lightAttackIndex = lightAttackIndex + 1 >= currentWeapon.lightAttack.Length? 0 : lightAttackIndex + 1;

        if(heavyAttackindex > 0)
        {
            ResetIndexes();
            lightAttackIndex = 0;
        }

        lastTimeAttacked = Time.time;
        Attack attack = currentWeapon.lightAttack[lightAttackIndex];
        lastAttack = attack;
        return attack;
    }

    public Attack SelectHeavyAttack()
    {
        if(!CanCombo())
            return currentWeapon.heavyAttack[0];

        heavyAttackindex = heavyAttackindex + 1 >= currentWeapon.heavyAttack.Length ? 0 : heavyAttackindex + 1; 

        if (lightAttackIndex > 0)
        {
            ResetIndexes();
            heavyAttackindex = 0;
        }
        
        lastTimeAttacked = Time.time;
        Attack attack = currentWeapon.heavyAttack[heavyAttackindex];
        lastAttack = attack;
        return attack;
    }

    public Attack SelectChargeAttack()
    {
        ResetIndexes();
        Attack attack = currentWeapon.chargeAttack;
        lastAttack = null;
        return attack;
    }

    public bool CanCombo()
    {
        if(lastAttack == null)
            return true;

        if(Time.time < lastTimeAttacked + lastAttack.clip.length + lastAttack.comboTime)
            return true;

        lastAttack = null;
        ResetIndexes();
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
        (currentWeapon, backupWeapon) = (backupWeapon, currentWeapon);
    }

    public void ChangeWeaponModel()
    {
        currentWeaponModel = Instantiate(currentWeapon.weaponDetails.model, weaponHolder);
    }

    public void ExecuteFableArt()
    {
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