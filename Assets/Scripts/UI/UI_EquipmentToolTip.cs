using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_EquipmentToolTip : UI_ItemToolTip
{
    [SerializeField] private GameObject modifierPanel;
    [SerializeField] private GameObject statNamePrefab;
    [SerializeField] private GameObject statValuePrefab;

    public override void ShowToolTip(ItemData item)
    {
        ShowEquipmentStats(item);
        base.ShowToolTip(item);
    }

    public void ShowEquipmentStats(ItemData item)
    {
        ItemData_Equipment equipment = item as ItemData_Equipment;

        foreach (var modifier in equipment.modifiers)
        {
            GameObject newStatName = Instantiate(statNamePrefab, modifierPanel.transform);
            newStatName.GetComponent<TextMeshProUGUI>().text = modifier.stat.ToString();

            string text = modifier.modType == StatModType.Flat ? modifier.value.ToString() : modifier.value.ToString() + " %";

            GameObject newStatValue = Instantiate(statValuePrefab, modifierPanel.transform);
            newStatValue.GetComponent<TextMeshProUGUI>().text = text;
        }
    }

    public override void HideToolTip()
    {
        base.HideToolTip();
        if (modifierPanel.TryGetComponent(out Transform _))
        {
            foreach (var child in modifierPanel.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject != modifierPanel)
                    Destroy(child.gameObject);
            }
        }
    }
}
