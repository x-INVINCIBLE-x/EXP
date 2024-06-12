using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    public List<Target> targets;
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
        targets.Remove(target);
    }

    public bool SelectTarget()
    {
        if(targets.Count == 0) return false;

        Target closestTarget = null;
        float closestDistance = float.PositiveInfinity;

        foreach(Target target in targets)
        {
            Vector2 viewPos = Camera.main.WorldToViewportPoint(target.transform.position);
            
            if(!target.GetComponentInChildren<Renderer>().isVisible)
                continue;

            Vector2 toCentre = viewPos - new Vector2(0.5f, 0.5f);
            if(toCentre.sqrMagnitude < closestDistance)
            {
                closestDistance = toCentre.sqrMagnitude;
                closestTarget = target;
            }
        }
        if (closestTarget == null) return false;

        currentTarget = closestTarget;
        targetGroup.AddMember(currentTarget.transform, 1, 2);
        return true;
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
}
