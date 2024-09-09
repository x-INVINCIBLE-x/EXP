using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TeleportTab : UI_Tab
{
    public Destination location;
    public Sprite sprite;

    public override void OnPointerDown(PointerEventData eventData)
    {
        //if (TeleportManager.instance.GetTeleportrersFrom(location).Count == 0)
        //    gameObject.SetActive(false);
        //else
        //    gameObject.SetActive(true);

        base.OnPointerDown(eventData);
        UI.instance.ShowTeleportLocation(location);
    }
}
