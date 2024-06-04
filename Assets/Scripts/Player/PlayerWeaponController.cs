using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponController
{
    public Weapon currentWeapon;
    private int lightAttackIndex = -1;
    private int heavyAttackindex = -1;
    private Attack lastAttack;
    private float lastTimeAttacked;

    public Attack SelectLightAttack()
    {
        if (!CanCombo())
            return currentWeapon.weaponData.lightAttack[0];

        lightAttackIndex = lightAttackIndex + 1 >= currentWeapon.weaponData.lightAttack.Length? 0 : lightAttackIndex + 1;

        if(heavyAttackindex > 0)
        {
            ResetIndexes();
        }

        Attack attack = currentWeapon.weaponData.lightAttack[lightAttackIndex];
        lastAttack = attack;
        return attack;
    }

    public Attack SelectHeavyAttack()
    {
        if(!CanCombo())
            return currentWeapon.weaponData.heavyAttack[0];

        heavyAttackindex = heavyAttackindex + 1 >= currentWeapon.weaponData.heavyAttack.Length ? 0 : heavyAttackindex + 1; 

        if (lightAttackIndex > 0)
        {
            ResetIndexes();
        }

        Attack attack = currentWeapon.weaponData.heavyAttack[heavyAttackindex];
        lastAttack = attack;
        return attack;
    }

    public Attack SelectChargeAttack()
    {
        ResetIndexes();
        Attack attack = currentWeapon.weaponData.chargeAttack;
        lastAttack = null;
        return attack;
    }

    public bool CanCombo()
    {
        if(lastAttack == null)
            return true;

        if(Time.time > lastTimeAttacked + lastAttack.clip.length * 0.9f + lastAttack.comboTime)
            return true;

        return false;
    }

    private void ResetIndexes()
    {
        lightAttackIndex = 0;
        heavyAttackindex = 0;
    }
}
