using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Fable Art/ Attack/ Charge Attack", fileName = "Charge Attack")]
public class ChargeFable : FableArt_Attack
{
    private float timer = 0;
    private float elapsedTime = 0;
    
    public override void Execute(int index = 0)
    {
        if(player.isBusy)
            return;

        CoroutineManager.Instance.StartRoutine(StartAttack());
    }

    private IEnumerator StartAttack()
    {
        player.SetBusy(true);
        yield return new WaitForEndOfFrame();

        int i = -1;
        InputManager input = player.inputManager;
        while (input.isHolding && i < holdAttacks.Length)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;

            if (timer < elapsedTime)
                continue;

            i++;

            if (i == holdAttacks.Length)
                break;

            player.anim.CrossFadeInFixedTime(holdAttacks[i].holdAnim.name, 0.051f, 0);
            if (i != holdAttacks.Length)
                elapsedTime += holdAttacks[i].duration;

        }

        i = Mathf.Clamp(i, 0, holdAttacks.Length - 1);
        
        player.SetBusy(false);
        player.stateMachine.ChangeState(new PlayerFableArtState(player.stateMachine, player, holdAttacks[i].attackAnim.AnimationName));
    }
}