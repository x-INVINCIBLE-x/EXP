using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fable Art/ Attack/ Multi Attack", fileName = "Multi Attack")]
public class FableArt_MultiAttack : FableArt
{
    public Attack[] attacks;

    public override void Execute(int index = 0)
    {
        StartAttack(index);
    }

    private void StartAttack(int index)
    {
        player.stateMachine.ChangeState(new PlayerFableArtState(player.stateMachine, player, attacks[index]));
    }
}