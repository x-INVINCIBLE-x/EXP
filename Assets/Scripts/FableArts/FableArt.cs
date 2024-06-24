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
    private readonly int animationSpeedHash = Animator.StringToHash("AttackSpeedMultiplier");

    public Player player => PlayerManager.instance.player;
    public FableArtType type;
    public int fableSlot;
    [Range(1, 3)]public float animationSpeedMultiplier;

    [System.Serializable]
    public class HoldAttack
    {
        public AnimationClip holdAnim;
        public Attack attackData;
        public float duration;
    }

    public virtual void Execute(int index = 0)
    {
        player.anim.SetFloat(animationSpeedHash, animationSpeedMultiplier);
    }
}
