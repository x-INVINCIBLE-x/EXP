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
    [field: SerializeField] public Destination Destination { get; private set; }
    [field: SerializeField] public Phase Phase { get; private set; }

    [field: SerializeField] public Transform SpawnPoint { get; private set; } // position to Teleport to
    public int BuildIndex { get; private set; }
    public bool IsActivaed { get; private set; }

    private UI ui;

    private void Start()
    {
        ui = UI.instance;
    }

    public override void Interaction()
    {
        //if (ui == null) 
        //    ui = UI.instance;

        ui.SetPortalUI(true);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        //if (!IsActivaed)
        //{
        //    BuildIndex = SceneManager.GetActiveScene().buildIndex;
        //    return;
        //}
    }

}
