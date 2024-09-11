using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Location to Teleport to, not own location
    [field: SerializeField] public Destination Destination { get; private set; }
    [field: SerializeField] public Phase Phase {  get; private set; }
    [field: SerializeField] public int BuildIndex {  get; private set; }
    [field: SerializeField] public Transform SpawnPoint { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() == null)
            return;
        TeleportManager.instance.TeleportToPortal(Destination, Phase, BuildIndex);
    }
}
