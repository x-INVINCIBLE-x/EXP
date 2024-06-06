using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Animator anim;
    private WeaponData currentWeapon;

    private Dictionary<EquipType, int> weaponEquipHash;
    private int idleHash = Animator.StringToHash("Idle");
    private int lightSwordDrawHash = Animator.StringToHash("Light Sword Draw");
    private int greatSwordDrawHash = Animator.StringToHash("Great Sword Draw");
    [Range(0.1f, 1f)]
    [SerializeField] private float transitionDuration = 0.1f;
    [SerializeField] private float weightDecRatel = 0.2f;
    [SerializeField] private float weightIncRate = 0.2f;

    private bool increaseWeight = false;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        weaponEquipHash = new Dictionary<EquipType, int>();
        weaponEquipHash[EquipType.Belt] = lightSwordDrawHash;
        weaponEquipHash[EquipType.Back] = greatSwordDrawHash;
    }

    private void Update()
    {
        UpdateLayerWeight();
    }

    public void SwitchWeapon(WeaponData newWeapon)
    {
        int index = ((int)newWeapon.weaponDetails.equipType);
        //float animLength = CurrentAnimationLenght(index);

        anim.CrossFadeInFixedTime(weaponEquipHash[newWeapon.weaponDetails.equipType], transitionDuration, 1);
        StartCoroutine(StartChangeLayerWeight(1.4f));

        currentWeapon = newWeapon;
    }

    private IEnumerator StartChangeLayerWeight(float time)
    {
        increaseWeight = true;

        yield return new WaitForSeconds(time);

        anim.CrossFade(idleHash, 1, 1);
        increaseWeight = false;
    }

    private void ChangeWeaponmodel()
    {

    }

    private void UpdateLayerWeight()
    {
        IncreaseLayerWeight(1);
        DecreaseLayerWeight(1);
    }

    public void IncreaseLayerWeight(int layerIndex)
    {
        float currentWeight = anim.GetLayerWeight(1);

        if (!increaseWeight || currentWeight >= 1)
            return;

        anim.SetLayerWeight(layerIndex, currentWeight + weightIncRate * Time.deltaTime);
    }

    public void DecreaseLayerWeight(int layerIndex)
    {
        float currentWeight = anim.GetLayerWeight(1);

        if (increaseWeight || currentWeight <= 0)
            return;

        anim.SetLayerWeight(layerIndex, currentWeight - weightDecRatel * Time.deltaTime);
    }

    protected float CurrentAnimationLenght(int index)
    {
        AnimatorStateInfo currentStateInfo = anim.GetCurrentAnimatorStateInfo(1);
        AnimatorClipInfo[] clipInfos = anim.GetCurrentAnimatorClipInfo(1);
        Debug.Log(clipInfos[0].clip.name);
        return clipInfos[index].clip.length;
    }
}
