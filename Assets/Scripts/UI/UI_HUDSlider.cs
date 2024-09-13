using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SliderStat
{
    Health,
    Stamina,
    Fable,
}

public class UI_HUDSlider : MonoBehaviour
{
    [SerializeField] protected Slider slider;
    [SerializeField] private SliderStat stat;
    protected UI_HUD uiHUD;

    protected virtual void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        uiHUD = GetComponentInParent<UI_HUD>();
    }

    protected virtual void Start()
    {
        if (!uiHUD.playerStat)
            uiHUD.playerStat = PlayerManager.instance.player.stats;

        uiHUD.playerStat.UpdateHUD += UpdateUI;
    }
    private void UpdateUI()
    {
        float[] values = uiHUD.sliderStats[stat]();
        slider.value = values[1] / values[0];
    }

    private void OnDisable()
    {
        uiHUD.playerStat.UpdateHUD += UpdateUI;
    }
}
