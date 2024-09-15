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
    private Dictionary<EffectData, List<GameObject>> activeEffects = new();


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

    public virtual void StartEffect(EffectData effect)
    {
        if (effect == null)
            return;

        GameObject newEffect = Instantiate(effect.effectItem, transform.position + effect.offestForEffects, Quaternion.identity);

        if (!activeEffects.ContainsKey(effect))
            activeEffects.Add(effect, new List<GameObject> { newEffect });
        else
            activeEffects[effect].Add(newEffect);

        newEffect.transform.parent = transform;
    }

    public virtual void RemoveEffect(EffectData effect)
    {
        if (effect == null)
            return;

        if (!activeEffects.ContainsKey(effect))
            return;

        foreach (GameObject toRemove in activeEffects[effect])
        {
            activeEffects.Remove(effect);
            Destroy(toRemove);

        }

        if (effect.residueEffect != null)
            Instantiate(effect.residueEffect, transform.position + effect.offestForEffects, Quaternion.identity);
    }
}
