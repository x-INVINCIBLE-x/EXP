using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
    //, ISaveable
{
    public string uniqueIdentifier;
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    [SerializeField] protected Sprite defaultImage;

    protected UI ui;
    public InventoryItem item;
    [SerializeField] private bool isSelected;
    [SerializeField] private GameObject selectionVisualizer;

    // CACHED STATE
    static Dictionary<string, UI_ItemSlot> globalLookup = new Dictionary<string, UI_ItemSlot>();

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

        if(selectionVisualizer != null)
            selectionVisualizer.SetActive(false);

        if (itemText)
            itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.clickCount == 0)
        {
            UI.instance.SelectSlot(this);
            isSelected = true;

            if (selectionVisualizer)
                selectionVisualizer.SetActive(isSelected);
        }
    }

    public void UnSelect()
    {
        isSelected = false;
        if (selectionVisualizer)
            selectionVisualizer.SetActive(isSelected);
    }

    public object CaptureState()
    {
        Debug.Log(item.data.itemId + "  " + item.data.name);
        InventorySlotRecord record = new InventorySlotRecord(item.data.itemId, item.stackSize);
        return record;
    }

    public virtual void RestoreState(object state)
    {
        InventorySlotRecord record = (InventorySlotRecord)state;
        ItemData data = Inventory.Instance.GetData(record.itemID);
        InventoryItem saavedItem = new InventoryItem(data);
        saavedItem.stackSize = record.amount;
        UpdateSlot(saavedItem);
    }

    [System.Serializable]
    public struct InventorySlotRecord
    {
        public string itemID;
        public int amount;

        public InventorySlotRecord(string id, int amt)
        {
            itemID = id;
            amount = amt;
        }
    }

    #region ID Generator
    public string GetUniqueIdentifier()
    {
        return uniqueIdentifier;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.IsPlaying(gameObject)) return;
        if (string.IsNullOrEmpty(gameObject.scene.path)) return;

        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

        if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
        {
            property.stringValue = System.Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }

        globalLookup[property.stringValue] = this;
    }
#endif

    private bool IsUnique(string candidate)
    {
        if (!globalLookup.ContainsKey(candidate)) return true;

        if (globalLookup[candidate] == this) return true;

        if (globalLookup[candidate] == null)
        {
            globalLookup.Remove(candidate);
            return true;
        }

        if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
        {
            globalLookup.Remove(candidate);
            return true;
        }

        return false;
    }
    #endregion

}
