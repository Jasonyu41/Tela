using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayCollider : MonoBehaviour
{
    [SerializeField] float delayTime;
    Collider[] colliders;
    Coroutine coroutine;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
    }

    private void OnEnable()
    {
        foreach (Collider coll in colliders)
        {
            coll.enabled = false;
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(enumerator());
    }

    private void OnDisable()
    {
        foreach (Collider coll in colliders)
        {
            coll.enabled = false;
        }
    }

    IEnumerator enumerator()
    {
        yield return new WaitForSeconds(delayTime);

        foreach (Collider coll in colliders)
        {
            coll.enabled = true;
        }
    }
}
