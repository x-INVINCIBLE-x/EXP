using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponController
{
    public WeaponData currentWeapon;
    private int lightAttackIndex = -1;
    private int heavyAttackindex = -1;
    private Attack lastAttack;
    private float lastTimeAttacked;

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
}