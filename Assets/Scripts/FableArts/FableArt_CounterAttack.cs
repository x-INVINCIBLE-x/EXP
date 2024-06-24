using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Fable Art/ Block/ Counter Block", fileName = "Counter Block")]
public class FableArt_CounterAttack : FableArt
{
    public float counterTimer = 1f;

    public override void Execute(int index = 0)
    {
        base.Execute(index);
        CoroutineManager.instance.StartRoutine(StartCounterCooldown());
    }

    public IEnumerator StartCounterCooldown()
    {
        player.canCounter = true;
        yield return new WaitForSeconds(counterTimer);
        player.canCounter = false;
    }
}
