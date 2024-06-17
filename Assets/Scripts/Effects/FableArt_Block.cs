using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fable Art/ Block/ Absolute Block", fileName = "Absolute Block")]
public class FableArt_Block : FableArt
{
    public float invincibleTime;
    public override void Execute(int index = 0)
    {
        player.GetComponent<PlayerStat>().SetInvincibleFor(invincibleTime);
        player.stateMachine.ChangeState(new PlayerBlockState(player.stateMachine, player, "Standing Block Idle", invincibleTime));
    }
}