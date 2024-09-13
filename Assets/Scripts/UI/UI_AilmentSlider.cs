using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AilmentSlider : MonoBehaviour
{
    [SerializeField] private string ailmentName;
    [SerializeField] protected Slider slider;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private AilmentType stat;
    [SerializeField] private Color initialColor;
    [SerializeField] private Color maxColor;
    private GameObject ailmentParent;
    protected UI_HUD uiHUD;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        uiHUD = GetComponentInParent<UI_HUD>();
        nameText = GetComponentInChildren<TextMeshProUGUI>();
        //ailmentParent = transform.GetChild(0).gameObject;
    }

    protected void Start()
    {
        nameText.text = ailmentName;
        ChangeColor(initialColor);
    }

    public IEnumerator UpdateUI()
    {
        var ailmentStatus = uiHUD.ailmentSliderStats[stat]();
        float currValue;

        do
        {
            float maxValue = ailmentStatus.ailmentLimit;
            currValue = ailmentStatus.Value;
            slider.value = (currValue / maxValue);

            bool isMaxed = uiHUD.ailmentSliderStats[stat]().isMaxed;
            if (isMaxed)
            {
                ChangeColor(maxColor);
            }


            if (uiHUD.ailmentSliderStats[stat]().Value <= 0)
            {
                ChangeColor(initialColor);
                gameObject.SetActive(false);
            }

            yield return new WaitForEndOfFrame();
        } while (currValue > 0);
    }

    private void ChangeColor(Color updatedColor)
    {
        ColorBlock cb = slider.colors;
        cb.normalColor = updatedColor;
        slider.colors = cb;
    }
}
