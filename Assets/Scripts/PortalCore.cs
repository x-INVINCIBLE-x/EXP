using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum Destination
{
    Level1, Level2, Level3, Level4, Level5, Level6
}

public enum Phase
{
    Phase1, Phase2, Phase3, Phase4, Phase5
}

public class PortalCore : Interactable
{
    [SerializeField] private string locationName;
    [SerializeField] private Sprite locationSprite;
    [field: SerializeField] public Destination Destination { get; private set; }
    [field: SerializeField] public Phase Phase { get; private set; }

    [field: SerializeField] public Transform SpawnPoint { get; private set; }

    private int buildIndex;
    private bool isActivaed;

    private UI ui;

    private void Start()
    {
        ui = UI.instance;
    }

    public override void Interaction()
    {
        //if (ui == null) 
        //    ui = UI.instance;

        if (!isActivaed)
        {
            buildIndex = SceneManager.GetActiveScene().buildIndex;
            TeleportManager.instance.AddTeleporter(locationName, Destination, Phase, buildIndex, locationSprite);
            isActivaed = true;
            return;
        }

        ui.SetPortalUITabs();
        ui.SetPortalUI(true);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

    }
}
