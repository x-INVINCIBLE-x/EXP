using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName ="Item/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponModel model;
    public Attack[] lightAttack;
    public Attack[] heavyAttack;
    public Attack chargeAttack;
    public Attack runLightAttack;
    public Attack runHeavyAttack;
    public float guardAmount;
}