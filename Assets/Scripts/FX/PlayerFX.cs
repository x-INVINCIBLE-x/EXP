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
}
