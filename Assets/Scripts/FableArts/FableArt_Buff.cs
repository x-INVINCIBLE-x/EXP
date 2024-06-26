using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fable Art/ Buff", fileName = "Buff")]
public class FableArt_Buff : FableArt
{
    public AnimationClip activateAnimation;
    public int duration;

    [Header("Buffs")]
    public int physicalAtk;
    public int physicalDef;
    public int stamina;

    public override void Execute(int index = 0)
    {
        base.Execute(index);

        if (activateAnimation != null)
            player.stateMachine.ChangeState(new PlayerFableArtState(player.stateMachine, player, activateAnimation.name));

        CoroutineManager.instance.StartRoutine(StartBuffEffect(player.stats));
    }

    private IEnumerator StartBuffEffect(CharacterStats stat)
    {
        AddBuffs(stat);

        yield return new WaitForSeconds(duration);

        RemoveBuffs(stat);
    }


    private void AddBuffs(CharacterStats stat)
    {
        if (physicalAtk != 0)
            stat.physicalAtk.AddModifier(new StatModifier(physicalAtk, StatModType.Flat, this));
        if (physicalDef != 0)
            stat.physicalDef.AddModifier(new StatModifier(physicalDef, StatModType.Flat, this));
        if (stamina != 0)
            stat.stamina.AddModifier(new StatModifier(stamina, StatModType.Flat, this));
    }
    private void RemoveBuffs(CharacterStats stat)
    {
        stat.physicalAtk.RemoveAllModifiersFromSource(this);
        stat.physicalDef.RemoveAllModifiersFromSource(this);
        stat.stamina.RemoveAllModifiersFromSource(this);
    }
}
