using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public CharacterStats characterStats;
    public WeaponData weaponData;

    private void Awake()
    {
        characterStats = GetComponentInParent<CharacterStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out CharacterStats enemyStats))
            return;


        characterStats.DoDamage(enemyStats);

        if(weaponData != null)
            characterStats.ChargeFable(weaponData.fableCharge);
    }
}
