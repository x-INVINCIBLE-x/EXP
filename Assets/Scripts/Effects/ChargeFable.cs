using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fable Art/ Attack/ Charge Attack", fileName = "Charge Attack")]
public class ChargeFable : FableArt_Attack
{
    private float timer;
    private float elapsedTime;
    public override void Execute(int index = 0)
    {
        CoroutineManager.Instance.StartRoutine(StartAttack());
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForEndOfFrame();
        int i = 0;
        InputManager input = player.inputManager;
        while (input.isHolding && i < holdAttacks.Length)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            if (timer > elapsedTime)
            {
                i++;
                if (i != holdAttacks.Length)
                    elapsedTime += holdAttacks[i].duration;
            }
        }

        i = holdAttacks.Length == i ? i - 1 : i;
        player.stateMachine.ChangeState(new PlayerFableArtState(player.stateMachine, player, holdAttacks[i].attackAnim.name));
    }
}