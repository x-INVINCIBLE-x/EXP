using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Tab : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject tabToOpen;

    public void OnPointerDown(PointerEventData eventData)
    {
        tabToOpen.SetActive(true);
    }
}
