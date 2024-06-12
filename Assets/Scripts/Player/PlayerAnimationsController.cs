using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour
{
    private Player player;
    [SerializeField] private GameObject weaponHolder;
    private Collider weaponCollider;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void ChangeWeaponTrigger()
    {
        player.ChangeWeaponModel();
    }

    public void ActivateWeaponCollission()
    {
        weaponCollider = weaponHolder.GetComponentInChildren<Collider>();
        weaponCollider.enabled = true;
    }

    public void DeactivateWeaponCollission()
    {
        weaponCollider.enabled = false;
        weaponCollider = null;
    }
}
