using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum FableArtType
{
    Attack,
    MultiAttack,
    Buff,
    Block
}

public class FableArt : ScriptableObject
{
    protected readonly int animationSpeedHash = Animator.StringToHash("AttackSpeedMultiplier");
    public GameObject[] effects;

    public Player player => PlayerManager.instance.player;
    public FableArtType type;
    public int fableSlot;

    [System.Serializable]
    public class HoldAttack
    {
        public AnimationClip holdAnim;
        public Attack attackData;
        public float duration;
    }

    public virtual void Execute(int index = 0)
    {

    }
}
