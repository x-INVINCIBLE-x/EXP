using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectionSlot : UI_ItemSlot
{
    [SerializeField] private Vector2 defaultCellSize;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    GridLayoutGroup group;

    private void Awake()
    {
        group = GetComponentInParent<GridLayoutGroup>(true);
    }

    public override void UpdateSlot(InventoryItem _newItem)
    {
        base.UpdateSlot(_newItem);

        ItemData_Equipment equipment = item.data as ItemData_Equipment;
        UpdateText(equipment);

        if (group == null) return;

        if (!equipment || equipment.subEquipmentType != EquipmentType.Weapon)
        {
            group.cellSize = defaultCellSize;
            return;
        }

        defaultCellSize = defaultCellSize == Vector2.zero ? group.cellSize : defaultCellSize;
        group.cellSize = new Vector2(100, 180);        
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (!itemImage.sprite)
            return;

        if(eventData.clickCount == 0)
        {
            UI.instance.ShowToolTip(item.data, true);
            return;
        }

        UI_SelectionSlotHandler currHandler = GetComponentInParent<UI_SelectionSlotHandler>();
        currHandler.gameObject.SetActive(false);
        Inventory.Instance.EquipItem(item.data, currHandler.parentSlot);


        UI.instance.HideToolTips();
    }

    private void UpdateText(ItemData_Equipment equipment)
    {
        if (itemTypeText == null) return;
        itemTypeText.text = equipment.subEquipmentType.ToString();
    }
}
