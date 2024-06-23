using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    private List<Target> targets = new List<Target>();
    public Target currentTarget {  get; private set; }

    [SerializeField]  private CinemachineTargetGroup targetGroup;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent(out Target target)) return;
        target.OnDestroyed += RemoveTarget;
        targets.Add(target);
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.TryGetComponent(out Target target)) return;
        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) return false;

        Target closestTarget = GetClosestTarget();

        if (closestTarget == null) return false;

        currentTarget = closestTarget;
        targetGroup.AddMember(currentTarget.transform, 1, 2);
        return true;
    }

    public Target GetClosestTarget()
    {
        Target closestTarget = null;
        float closestDistance = float.PositiveInfinity;

        foreach (Target target in targets)
        {
            Vector2 viewPos = Camera.main.WorldToViewportPoint(target.transform.position);

            if (!target.GetComponentInChildren<Renderer>().isVisible)
                continue;

            Vector2 toCentre = viewPos - new Vector2(0.5f, 0.5f);
            if (toCentre.sqrMagnitude < closestDistance)
            {
                closestDistance = toCentre.sqrMagnitude;
                closestTarget = target;
            }
        }

        return closestTarget;
    }

    public void RemoveTarget(Target target)
    {
        if (target == currentTarget)
        {
            targetGroup.RemoveMember(currentTarget.transform);
            currentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }

    public void RemoveCurrentTarget()
    {
        if (currentTarget == null) return;

        targetGroup.RemoveMember(currentTarget.transform);
        currentTarget = null;
    }

    public List<Target> GettargetList() => targets;
}
