using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Fable Art/ Attack/ Charge Attack", fileName = "Charge Attack")]
public class FableArt_ChargeAttack : FableArt_Attack
{
    public HoldAttack[] chargeAttacks;

    private float timer = 0;
    private float elapsedTime = 0;
    
    public override void Execute(int index = 0)
    {
        if(player.isBusy)
            return;

        base.Execute(index);

        CoroutineManager.instance.StartRoutine(StartAttack());
    }

    private IEnumerator StartAttack()
    {
        player.SetBusy(true);
        player.SetCanMove(false);
        yield return new WaitForEndOfFrame();

        int i = -1;
        InputManager input = player.inputManager;
        while (input.isHolding && i < chargeAttacks.Length)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;

            if (timer < elapsedTime)
                continue;

            i++;
            if (i == chargeAttacks.Length)
                break;

            StartEffectAt(i);
            player.anim.CrossFadeInFixedTime(chargeAttacks[i].holdAnim.name, 0.51f, 0);
            if (i != chargeAttacks.Length)
                elapsedTime += chargeAttacks[i].duration;

        }

        i = Mathf.Clamp(i, 0, chargeAttacks.Length - 1);
        
        player.SetBusy(false);
        StopAllEffects();
        player.stateMachine.ChangeState(new PlayerFableArtState(player.stateMachine, player, chargeAttacks[i].attackData, animationSpeedMultiplier, effects[i]));
        yield return null;
    }
}