using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [SerializeField] private GameObject playerEffects;
    [SerializeField] private GameObject effectPrefab;
    [Tooltip("Fire, Electric, Acid, Disruption, Shock, Break")] 
    [SerializeField] private List<Material> ailmentMaterials;

    private readonly Dictionary<int, GameObject> appliedEffects = new();

    public void ApplyEffectFX(AilmentType type, CharacterStats.AilmentStatus ailmentStatus)
    {
        int code = ailmentStatus.GetHashCode();
        if (appliedEffects.ContainsKey(code))
            return;

        GameObject newEffect = Instantiate(effectPrefab, playerEffects.transform);
        Renderer renderer = newEffect.GetComponent<Renderer>();
        renderer.material = ailmentMaterials[((int)type)];

        appliedEffects[code] = newEffect;
    }

    public void RemoveEffectFX(CharacterStats.AilmentStatus ailmentStatus)
    {
        GameObject effectToRemove = appliedEffects[ailmentStatus.GetHashCode()];
        appliedEffects.Remove(ailmentStatus.GetHashCode());
        Destroy(effectToRemove);
    }
}
