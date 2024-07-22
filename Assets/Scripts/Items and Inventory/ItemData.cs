using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ItemType
{
    UsableItem,
    Material,
    Equipment
}


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;

    [Space]
    public string itemName;
    public Sprite itemIcon;
    public string itemId;

    [TextArea]
    [SerializeField] private string description;

    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        return description;
    }

    [System.Serializable]
    public class Modifier
    {
        public Stats stat;
        public StatModType modType;
        public float value;
    }
}
