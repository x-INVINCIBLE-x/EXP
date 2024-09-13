using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterStats;

public class UI_HUD : MonoBehaviour
{
    public PlayerStat playerStat;
    public Dictionary<SliderStat, Func<float[]>> sliderStats;
    public Dictionary<AilmentType, Func<AilmentStatus>> ailmentSliderStats;

    public void Start()
    {
        playerStat = PlayerManager.instance.player.stats;

        sliderStats = new Dictionary<SliderStat, Func<float[]>>
        {
            {SliderStat.Health, () => new  float[] {playerStat.health.Value, playerStat.currentHealth} },
            {SliderStat.Stamina,() => new float[] {playerStat.stamina.Value, playerStat.currentStamina} },
            {SliderStat.Fable, () => new float[] { playerStat.fableSlot.Value, playerStat.currentFableSlot } }
        };

        ailmentSliderStats = new Dictionary<AilmentType, Func<AilmentStatus>>()
        {
            {AilmentType.Fire, () => playerStat.fireStatus},
            {AilmentType.Electric, () => playerStat.electricStatus},
            {AilmentType.Acid, () => playerStat.acidStatus},
            {AilmentType.Break, () => playerStat.breakStatus},
            {AilmentType.Disruption, () => playerStat.disruptionStatus},
            {AilmentType.Shock, () => playerStat.shockStatus}
        };
    }
}
