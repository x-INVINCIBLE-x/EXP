using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Panel : MonoBehaviour
{
    private UI ui => UI.instance;

    private void OnEnable()
    {
        ui.AddToActivePanels(this);
    }

    private void OnDisable()
    {
        ui.RemoveFromActivePanel(this);
    }
}
