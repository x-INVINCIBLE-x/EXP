using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public Player player => PlayerManager.instance.player;
    public FableArtType type;
    public int fableSlot;

    public List<EffectData>  effects;
    private List<GameObject> activeEffects;

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

    public virtual void StartAllEffects()
    {
        for (int i = 0; i < effects.Count; i++)
            activeEffects.Add(Instantiate(effects[i].effectItem, player.transform.position + effects[i].offestForEffects, Quaternion.identity));
    }

    public virtual void StartEffectAt(int index)
    {
        if (index >= effects.Count)
        {
            Debug.LogWarning(name + " out of bound fable effect Call");
            return;
        }

        activeEffects.Add(Instantiate(effects[index].effectItem, player.transform.position + effects[index].offestForEffects, Quaternion.identity));
    }

    public virtual void StopEffects()
    {
        if (activeEffects == null)
        {
            Debug.Log("No Effect To Destroy");
            return;
        }

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            if (effects[i].residueEffect != null)
                Instantiate(effects[i].residueEffect, player.transform.position + effects[i].offestForEffects, Quaternion.identity);

            GameObject effectToDelete = activeEffects[i];

            activeEffects.RemoveAt(i);
            Destroy(effectToDelete);
        }
    }
}
