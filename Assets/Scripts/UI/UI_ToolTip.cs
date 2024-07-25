using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ToolTip : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public Image itemImage;

    public TextMeshProUGUI descriptionHeadingText;
    public TextMeshProUGUI itemEffectText;
    public TextMeshProUGUI itemDescription;

    public virtual void ShowToolTip(ItemData item)
    {
        itemName.text = item.name;
        itemImage.sprite = item.itemIcon;

        descriptionHeadingText.text = item.itemType == ItemType.UsableItem ? "Use Effect" : "Purpose";
        itemEffectText.text = item.shortDescription;
        itemDescription.text = item.GetDescription();

        gameObject.SetActive(true);
    }

    public virtual void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
