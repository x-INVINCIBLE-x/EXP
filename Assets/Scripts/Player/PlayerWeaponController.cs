using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponController
{
    public Weapon currentWeapon;
    private int lightAttackIndex = -1;
    private int heavyAttackindex = -1;

    public Attack SelectLightAttack()
    {
        lightAttackIndex = lightAttackIndex + 1 >= currentWeapon.weaponData.lightAttack.Length? 0 : lightAttackIndex + 1;

        if(heavyAttackindex > 0)
        {
            lightAttackIndex = 0;
            heavyAttackindex = 0;
        }
        
        return currentWeapon.weaponData.lightAttack[lightAttackIndex];
    }

    public void SelectHeavyAttack()
    {

    }

    public void SelectChargeAttack()
    {

    }
}
