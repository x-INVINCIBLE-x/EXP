using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fable Art/ Block/ Absolute Block", fileName = "Absolute Block")]
public class FableArt_AbsoluteBlock : FableArt
{
    public AnimationClip absoluteBlockCLip;
    public float invincibleTime;

    public override void Execute(int index = 0)
    {
        base.Execute(index);

        player.GetComponent<PlayerStat>().SetInvincibleFor(invincibleTime);

        string blockAnimName = "Standing Block Idle";

        if(absoluteBlockCLip != null)
            blockAnimName = absoluteBlockCLip.name;

        CoroutineManager.instance.StartRoutine(Effect(invincibleTime));
        player.stateMachine.ChangeState(new PlayerBlockState(player.stateMachine, player, blockAnimName, invincibleTime));
    }

    IEnumerator Effect(float duraion)
    {
        StartAllEffects();
        yield return new WaitForSeconds(duraion);
        StopAllEffects();
    }
}