using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HUD : MonoBehaviour
{
    protected PlayerStat playerStat;
    protected Dictionary<SliderStat, Func<float[]>> sliderStats;

    protected virtual void Start()
    {
        playerStat = PlayerManager.instance.player.stats;

        sliderStats = new Dictionary<SliderStat, Func<float[]>>
        {
            {SliderStat.Health, () => new  float[] {playerStat.health.Value, playerStat.currentHealth} },
            {SliderStat.Stamina,() => new float[] {playerStat.stamina.Value, playerStat.currentStamina} },
            {SliderStat.Fable, () => new float[] { playerStat.fableSlot.Value, playerStat.currentFableSlot } }
        };
    }
}
