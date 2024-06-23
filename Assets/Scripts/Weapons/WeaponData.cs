using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName ="Item/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
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
