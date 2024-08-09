using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ToolTip : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    public virtual void ShowToolTip(ItemData item)
    {

    }

    public virtual void HideToolTip()
    {

    }
}
