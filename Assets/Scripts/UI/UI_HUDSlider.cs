using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SliderStat
{
    Health,
    Stamina,
    Fable 
}

public class UI_HUDSlider : UI_HUD
{
    [SerializeField] private Slider slider;
    [SerializeField] private SliderStat stat;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    protected override void Start()
    {
        base.Start();
        playerStat.Hit += UpdateUI;
    }
    private void UpdateUI()
    {
        float[] values = sliderStats[stat]();
        slider.value = values[1] / values[0];
    }

    private void OnDisable()
    {
        playerStat.Hit += UpdateUI;
    }
}
