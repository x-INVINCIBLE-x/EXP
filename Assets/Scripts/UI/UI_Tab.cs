using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UI_Tab : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected string tabName;
    [SerializeField] [TextArea] protected string tabDescription;
    [SerializeField] protected GameObject tabToOpen;

    [SerializeField] protected TextMeshProUGUI nameDisplay;
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        UI.instance.DeselectSlot();

        tabToOpen.SetActive(true);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        UI.instance.panelToolTip.ShowToolTip(tabName, tabDescription);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        UI.instance.panelToolTip.HideToolTip();
    }
}
