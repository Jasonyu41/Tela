using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1_Attack_VFX : MonoBehaviour
{
    [SerializeField] AttackDatas[] attackDatas;
    [SerializeField] bool isSustainedDamage;
    [SerializeField] float sustainedDamageIntervalTime;
    [SerializeField] protected string targetTag = "Player";

    AttackData[] attacks;
    WaitForSeconds[] waitForSeconds;
    WaitForSeconds waitForSustainedDamageIntervalTime;
    protected Transform tf;
    protected Collider coll;

    protected virtual void Awake()
    {
        waitForSeconds = new WaitForSeconds[attackDatas.Length];
        for (int i = 0; i < attackDatas.Length; i++)
        {
            waitForSeconds[i] = new WaitForSeconds(attackDatas[i].attackIntervalsTime);
        }
        waitForSustainedDamageIntervalTime = new WaitForSeconds(sustainedDamageIntervalTime);

        tf = transform;
        coll = GetComponent<Collider>();
        coll.enabled = false;
    }

    protected virtual void OnEnable()
    {
        coll.enabled = true;

        StartCoroutine(ChangeAttacks());
    }

    protected virtual void OnDisable()
    {
        coll.enabled = false;

        StopCoroutine(ChangeAttacks());
    }

    IEnumerator ChangeAttacks()
    {
        for (int i = 0; i < attackDatas.Length; i++)
        {
            attacks = attackDatas[i].attackDatas;

            yield return waitForSeconds[i];
        }
    }

    IEnumerator SwitchCollider()
    {
        coll.enabled = false;

        yield return waitForSustainedDamageIntervalTime;

        coll.enabled = true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            foreach (AttackData attack in attacks)
            {
                AttackManager.NormalAttack(other.GetComponent<Character>(), attack);

                if (isSustainedDamage) StartCoroutine(SwitchCollider());
            }
        }
    }
}
