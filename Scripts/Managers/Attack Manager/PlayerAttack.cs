using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float damageMagnification = 1;
    [SerializeField] float damageAddition = 0;
    [SerializeField] int damageCount = 1;
    [SerializeField] float damageDelay;
    [SerializeField] public string targetTag = "Enemy";

    [HideInInspector] AttackData physicalAttack;
    [HideInInspector] AttackData magicAttack;
    [HideInInspector] AttackData attribute;
    [HideInInspector] public bool isAttackHit;
    Collider[] colliders;
    WaitForFixedUpdate waitForFixedUpdate;
    Coroutine attackCoroutine;
    float timer;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        isAttackHit = false;
        foreach (Collider coll in colliders)
        {
            coll.enabled = true;
        }
    }

    public void Initinlize(AttackData physicalAttack, AttackData magicAttack, AttackData attribute)
    {
        this.physicalAttack = physicalAttack;
        this.magicAttack = magicAttack;
        this.attribute = attribute;

        this.physicalAttack.damage *= damageMagnification;
        this.magicAttack.damage *= damageMagnification;
        this.attribute.damage *= damageMagnification;

        this.physicalAttack.damageAddition = damageMagnification * (this.physicalAttack.damageAddition + damageAddition);
        this.magicAttack.damageAddition = damageMagnification * (this.magicAttack.damageAddition + damageAddition);
        this.attribute.damageAddition = damageMagnification * (this.attribute.damageAddition + damageAddition);
    }

    public void Attack(Collider other)
    {
        if (isAttackHit) return;
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackCoroutine(other));
    }

    IEnumerator AttackCoroutine(Collider other)
    {
        isAttackHit = true;
        for (int i = 0; i < damageCount; i++)
        {
            AttackManager.NormalAttack(other.GetComponent<Character>(), physicalAttack);
            AttackManager.NormalAttack(other.GetComponent<Character>(), magicAttack);
            AttackManager.NormalAttack(other.GetComponent<Character>(), attribute);

            if (damageDelay > 0)
            {
                timer = 0;
                while (timer < damageDelay)
                {
                    timer += Time.fixedDeltaTime;

                    yield return waitForFixedUpdate;
                }
            }
        }
        yield return null;
    }
}
