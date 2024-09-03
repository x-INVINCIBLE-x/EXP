using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Teleport : MonoBehaviour, IPointerDownHandler
{
    public PortalCore portal;

    public Destination destination;
    public Phase phase;
    public int buildIndex;

    private void Awake()
    {
        //portal = GetComponentInParent<PortalCore>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        portal.TeleportTo(destination, phase, buildIndex);
    }
}
