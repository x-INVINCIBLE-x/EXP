using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fable Art/ Attack/ Slash", fileName = "Slash")]
public class FableArt_Slashes : FableArt_Attack
{
    public Attack[] attacks;

    public override void Execute(int index = 0)
    {
        base.Execute(index);
        CoroutineManager.instance.StartRoutine(StartAttack());
    }

    private IEnumerator StartAttack()
    {
        int i = 0;
        StartAllEffects();

        while (i < attacks.Length)
        {
            if (i < effects.Count && effects[i] != null)
                player.stateMachine.ChangeState(new PlayerFableArtState(player.stateMachine, player, attacks[i], animationSpeedMultiplier, effects[i]));
            else
                player.stateMachine.ChangeState(new PlayerFableArtState(player.stateMachine, player, attacks[i], animationSpeedMultiplier));
            yield return new WaitForSeconds(attacks[i].clip.length * 0.6f);
            i++;
        }
        StopAllEffects();
        yield return null;
    }
}
