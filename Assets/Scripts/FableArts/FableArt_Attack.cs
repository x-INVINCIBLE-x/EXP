using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FableArt_Attack : FableArt
{
    [Range(1, 3)] public float animationSpeedMultiplier;

    public override void Execute(int index = 0)
    {
        base.Execute(index);

        player.anim.SetFloat(animationSpeedHash, animationSpeedMultiplier);
    }
}
