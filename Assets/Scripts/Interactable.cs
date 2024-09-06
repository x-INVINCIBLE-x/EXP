using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interaction()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null)
            return;

        playerInteraction.GetInteracbles().Add(this);
        playerInteraction.UpdateClosestInteractble();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null)
            return;

        playerInteraction.GetInteracbles().Remove(this);
        playerInteraction.UpdateClosestInteractble();
    }
}
