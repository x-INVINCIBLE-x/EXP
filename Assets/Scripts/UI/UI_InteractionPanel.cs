using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InteractionPanel : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
{
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject moveToEquipmentButton;
    [SerializeField] private GameObject destroyButton;
    private ItemData_Usable usableItem;

    private bool canUse = false;
    private bool canMoveToEquipment = false;
    private bool canBeDestroyed = false;

    private float closeCooldown = 1.5f;
    private float lastTimeEnter = 0f;

    [SerializeField] private Vector3 offset = Vector3.zero;
    public void Setup(bool canUse, bool canMoveToEquipment, bool canBeDestroyed, ItemData_Usable usableItem = null)
    {
        this.canUse = canUse;
        this.canMoveToEquipment = canMoveToEquipment;
        this.canBeDestroyed = canBeDestroyed;
        this.usableItem = usableItem;
    }

    private void Update()
    {
        if (Time.time > lastTimeEnter + closeCooldown)
            Hide();
    }

    public void Show(Transform itemSlotTransform)
    {
        lastTimeEnter = Time.time;
        if ( !canUse && !canMoveToEquipment && !canBeDestroyed )
        {
            Hide();
            return;
        }

        transform.position = itemSlotTransform.position + offset;

        useButton.SetActive(canUse);
        moveToEquipmentButton.SetActive(canMoveToEquipment);
        destroyButton.SetActive(canBeDestroyed);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Use()
    {
        if (!usableItem)
            return;

        usableItem.UseItem(PlayerManager.instance.player.stats);
    }

    public void MoveToEquipment()
    {
        UI.instance.SwitchTo(Panels.equipmentPanel);
    }

    public void Destroy()
    {
        if (!usableItem)
            return;

        Inventory.Instance.RemoveItem(usableItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        lastTimeEnter = Time.time;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        lastTimeEnter = Time.time;
    }
}