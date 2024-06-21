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
    public Player player => PlayerManager.instance.player;
    public FableArtType type;

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
