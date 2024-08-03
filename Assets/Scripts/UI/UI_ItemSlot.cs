using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    [SerializeField] protected Sprite defaultImage;

    protected UI ui;
    public InventoryItem item;
    [SerializeField] private bool isSelected;

    private void Awake()
    {
        itemText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public virtual void UpdateSlot(InventoryItem _newItem)
    {
        if (_newItem == null || _newItem.data == null)
        {
            CleanUpSlot();
            return;
        }

        item = _newItem;

        itemImage.color = Color.white;

        itemImage.sprite = item.data.itemIcon;

        if (itemText)
            UpdateText();
    }

    private void UpdateText()
    {
        if (item.stackSize > 1)
        {
            itemText.text = item.stackSize.ToString();
        }
        else
        {
            itemText.text = "";
        }
    }

    public virtual void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        if (itemText)
            itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.clickCount == 0)
        {
            UI.instance.SelectSlot(this);
            isSelected = true;
        }
    }

    public void UnSelect()
    {
        isSelected = false;
    }

}
