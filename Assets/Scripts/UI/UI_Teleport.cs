using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Teleport : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image displayImage;
    [SerializeField] private TextMeshProUGUI nameText;

    public string locationName;
    public Destination location;
    public Phase phase;
    public int buildIndex;
    public Sprite sprite;

    public void UpdateSlot(string locationName, Destination location, Phase phase, int buildIndex, Sprite sprite = null)
    {
        this.locationName = locationName;
        this.location = location;
        this.phase = phase;
        this.buildIndex = buildIndex;
        this.sprite = sprite;

        nameText.text = locationName;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.clickCount == 0)
        {
            if (displayImage != null)
                displayImage.sprite = sprite;
            return;
        }

        TeleportManager.instance.TeleportToTeleportal(location, phase, buildIndex);
    }

    public void CleanSlot()
    {
        nameText.text = "";
    }
}
