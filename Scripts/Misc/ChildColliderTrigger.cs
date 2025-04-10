using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildColliderTrigger : MonoBehaviour
{
    [SerializeField] bool tempbool = false;
    [SerializeField] float tempTime = 0;

    PlayerAttack playerAttack;
    Collider coll;
    float timer;

    WaitForFixedUpdate waitForFixedUpdate;

    private void Awake()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
        coll = GetComponent<Collider>();
        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        coll.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerAttack.targetTag)
        {
            playerAttack.Attack(other);

            if (tempbool)
            {
                StartCoroutine(enumerator());
            }
        }
    }

    IEnumerator enumerator()
    {
        coll.enabled = false;
        
        timer = 0;
        while (timer < tempTime)
        {
            timer += Time.fixedDeltaTime;

            yield return waitForFixedUpdate;
        }

        coll.enabled = true;
        playerAttack.isAttackHit = false;
    }
}
