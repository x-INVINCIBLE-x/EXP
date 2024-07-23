using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Usable Item", menuName = "Data/Usable Item")]
public class ItemData_Usable : ItemData
{
    public string itemEffectDescription;

    [SerializeField] private List<Modifier> modifiers;
    [SerializeField] private List<ItemEffect> effects;

    [SerializeField] private float timer = -1f;

    public void UseItem(CharacterStats stats)
    {
        Debug.Log("Item Used");
        AddModifiers(stats);

        ApplyEffects();
    }

    private void AddModifiers(CharacterStats stats)
    {
        if (modifiers.Count == 0)
            return;

        foreach (Modifier modifier in modifiers)
        {
            if (stats.statDictionary.TryGetValue(modifier.stat, out Stat stat))
            {
                StatModifier statMod = new StatModifier(modifier.value, modifier.modType, this);
                stat.AddModifier(statMod);
            }
            else
            {
                Debug.LogWarning("Stat not found" + modifier.stat);
            }
        }

        if (timer > 0f)
            CoroutineManager.instance.StartRoutine(RemoveModifiers(stats));
    }

    private void ApplyEffects()
    {
        if (effects.Count == 0)
            return;

        foreach (ItemEffect effect in effects)
        {
            effect.ExecuteEffect();
        }
    }


    private IEnumerator RemoveModifiers(CharacterStats stats)
    {
        yield return new WaitForSeconds(timer);

        foreach (Stat stat in stats.statDictionary.Values)
        {
            stat.RemoveAllModifiersFromSource(this);
        }
    }
}
