using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Teleport : MonoBehaviour, IPointerDownHandler
{
    public Destination destination;
    public Phase phase;
    public int buildIndex; 

    public void OnPointerDown(PointerEventData eventData)
    {
        TeleportManager.instance.TeleportTo(destination, phase, buildIndex);
    }
}
