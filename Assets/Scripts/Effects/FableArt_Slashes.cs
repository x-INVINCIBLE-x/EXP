using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fable Art/ Attack/ Slash", fileName = "Slash")]
public class FableArt_Slashes : FableArt_Attack
{
    public override void Execute(int index = 0)
    {
        CoroutineManager.Instance.StartRoutine(StartAttack());
    }

    private IEnumerator StartAttack()
    {
        int i = 0;
        while (i < attacks.Length)
        {
            player.stateMachine.ChangeState(new PlayerFableArtState(player.stateMachine, player, attacks[i]));
            yield return new WaitForSeconds(attacks[i].clip.length);
            i++;
        }
    }
}
