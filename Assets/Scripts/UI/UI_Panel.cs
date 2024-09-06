using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Panel : MonoBehaviour
{
    [SerializeField] private UI ui;

    private void Awake()
    {
        ui = GetComponentInParent<UI>(true);
    }

    private void OnEnable()
    {
        ui.AddToActivePanels(this);
    }

    private void OnDisable()
    {
        ui.RemoveFromActivePanel(this);
    }
}
