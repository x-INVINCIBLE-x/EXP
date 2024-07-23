using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Tab : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject tabToOpen;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (tabToOpen.TryGetComponent(out UI_BagInternalPanels internalPanel))
        {
            UI.instance.SwitchBagPanel(internalPanel);
            return;
        }

        tabToOpen.SetActive(true);
    }
}
