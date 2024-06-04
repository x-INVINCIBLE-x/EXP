using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public AnimationClip clip;
    public string AnimationName => clip.name;
    public float PhysicalATK;
    public float TransitionTime;
    public float movementSpeed;
    public float comboTime;
}
