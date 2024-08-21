using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "WeaponData", menuName ="Item/Weapon")]
public class WeaponData : ItemData_Equipment
{
    public WeaponModel weaponDetails;
    public Attack[] lightAttack;
    public Attack[] heavyAttack;
    public Attack chargeAttack;
    public Attack sprintLightAttack;
    public Attack sprintHeavyAttack;
    public int fableCharge;
    public float guardAmount;

    public float fireAtk;
    public float electricAtk;
    public float acidAtk;

    public AnimationClip counterAttackAnim;

    public FableArt fableBlade;
}
