using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PanelToolTip : UI_ToolTip
{
    public void ShowToolTip(string name, string description)
    {
        itemName.text = name;
        itemDescription.text = description;
        gameObject.SetActive(true);
    }

    public override void HideToolTip()
    {
        base.HideToolTip();
        gameObject.SetActive(false);
    }
}
