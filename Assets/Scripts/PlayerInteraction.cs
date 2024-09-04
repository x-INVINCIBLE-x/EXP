using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject interactUI;

    private List<Interactable> interactables = new List<Interactable>();
    private Interactable closestInteractable;

    private void Start()
    {
        Player player = GetComponent<Player>();
        //player.inputManager.Interaction.performed += context => InteractWithClosest();
    }

    private void Update()
    {
        if (interactables.Count > 0)
            interactUI.SetActive(true);
        else 
            interactUI.SetActive(false);
    }

    private void InteractWithClosest()
    {
        closestInteractable?.Interaction();
        interactables.Remove(closestInteractable);

        UpdateClosestInteractble();
    }

    public void UpdateClosestInteractble()
    {
        //closestInteractable?.HighlightActive(false);
        closestInteractable = null;

        float closestDistance = float.MaxValue;

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        if (interactables.Count > 0)
            interactUI.SetActive(true);
        else
            interactUI.SetActive(false);

        //closestInteractable?.HighlightActive(true);
    }


    public List<Interactable> GetInteracbles() => interactables;
}
