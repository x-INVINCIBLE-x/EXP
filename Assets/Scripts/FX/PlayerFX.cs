using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [SerializeField] private Transform playerEffects;
    [SerializeField] private GameObject effectPrefab;
    [Tooltip("Fire, Electric, Acid, Disruption, Shock, Break")] 
    [SerializeField] private List<Material> ailmentMaterials;

    private readonly Dictionary<int, GameObject> appliedEffects = new();
    private int dir = -1;

    [Header("Fable Fx")]
    private List<EffectData> effects = new();
    private Dictionary<EffectData, GameObject> activeEffects = new();


    public void ApplyEffectFX(AilmentType type, CharacterStats.AilmentStatus ailmentStatus)
    {
        int code = ailmentStatus.GetHashCode();
        if (appliedEffects.ContainsKey(code))
            return;

        GameObject newEffect = Instantiate(effectPrefab, playerEffects);
        Renderer renderer = newEffect.GetComponent<Renderer>();
        renderer.material = ailmentMaterials[((int)type)];
        newEffect.transform.localPosition =
            new(0, 0, (appliedEffects.Count) * -0.05f);

        dir *= -1;
        newEffect.GetComponent<RotationEffect>().Setup(dir);
        appliedEffects[code] = newEffect;
    }

    public void RemoveEffectFX(CharacterStats.AilmentStatus ailmentStatus)
    {
        GameObject effectToRemove = appliedEffects[ailmentStatus.GetHashCode()];
        appliedEffects.Remove(ailmentStatus.GetHashCode());
        Destroy(effectToRemove);
    }

    public virtual void StartEffectAt(int index)
    {
        if (index >= effects.Count)
        {
            Debug.LogWarning("out of bound fable effect from fable attackl Call");
            return;
        }

        activeEffects.Add(effects[index], Instantiate(effects[index].effectItem, transform.position + effects[index].offestForEffects, Quaternion.identity));
    }

    public virtual void StartEffect(EffectData effect)
    {
        if (effect == null)
            return;

        Debug.Log(effect.effectItem.name);
        activeEffects.Add(effect, Instantiate(effect.effectItem, transform.position + effect.offestForEffects, Quaternion.identity));
    }

    public virtual void RemoveEffect(EffectData effect)
    {
        if(effect == null)
            return;

        if (activeEffects.ContainsKey(effect))
        {
            GameObject objectToRemove = activeEffects[effect];
            activeEffects.Remove(effect);
            Destroy(objectToRemove);
        }
    }


    public virtual void RemoveEffects()
    {
        if (activeEffects == null)
        {
            Debug.Log("No Effect To Destroy");
            return;
        }

        foreach(var effect in activeEffects)
        {
            GameObject objecttoRemove = effect.Value;
            activeEffects.Remove(effect.Key);
            Destroy(objecttoRemove);
        }
    }
}
