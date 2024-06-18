using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum FableArtType
{
    Slashes,
    MultiAttack,
    Hold,
    Buff
}

public class FableArt : ScriptableObject
{
    public Player player => PlayerManager.instance.player;
    public FableArtType type;
    public HoldAttack[] holdAttacks;
    public bool canHold;

    [System.Serializable]
    public class HoldAttack
    {
        public AnimationClip holdAnim;
        public Attack attackAnim;
        public float duration;
    }

    public virtual void Execute(int index = 0)
    {

    }
}
