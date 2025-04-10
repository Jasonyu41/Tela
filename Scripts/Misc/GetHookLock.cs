using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHookLock : MonoBehaviour
{
    [SerializeField] Vector3 detectionCenter;
    [SerializeField] Vector3 detectionRange;
    [SerializeField] LayerMask detectionLayer;

    public HookLockTrigger DetectionHookLock(Vector3 playerPosition)
    {
        Collider[] colliders = Physics.OverlapBox(transform.TransformPoint(detectionCenter), detectionRange, Quaternion.identity, detectionLayer);

        float closestDistance = float.MaxValue;
        HookLockTrigger closestHookLock = null;

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<HookLockTrigger>(out var hookLockTrigger))
            {
                if (!hookLockTrigger.isInVisualField) continue;
                var temp = Vector3.Distance(transform.position, hookLockTrigger.highestPoint.position);
                if (Physics.Raycast(playerPosition, (hookLockTrigger.highestPoint.position - playerPosition).normalized, out RaycastHit hit, temp - 1))
                {
                    #if UNITY_EDITOR
                    print("有障礙物: " + hit.transform.name);
                    #endif
                    continue;
                }
                if (temp < closestDistance)
                {
                    closestDistance = temp;
                    closestHookLock = hookLockTrigger;
                }
            }
        }

        if (closestHookLock != null)
        {
            return closestHookLock;
        }
        else
        {
            return null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.TransformPoint(detectionCenter), detectionRange * 2);
    }
#endif

}
