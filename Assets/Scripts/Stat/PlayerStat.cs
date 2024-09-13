using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStats
{
    [SerializeField] private PlayerFX fx;

    protected override void Awake()
    {
        base.Awake();
        fx = GetComponent<PlayerFX>();
    }

    protected override void TryApplyAilmentEffect(float ailmentAtk, ref AilmentStatus ailmentStatus, AilmentType ailmentType)
    {
        base.TryApplyAilmentEffect(ailmentAtk, ref ailmentStatus, ailmentType);
        if (ailmentStatus.Value >= ailmentStatus.ailmentLimit)
            fx.ApplyEffectFX(ailmentType, ailmentStatus);
    }

    protected override void AilmentEffectEnded(AilmentStatus ailmentStatus)
    {
        base.AilmentEffectEnded(ailmentStatus);
        fx.RemoveEffectFX(ailmentStatus);
    }
}
