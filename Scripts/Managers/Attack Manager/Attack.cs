using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] AttackDatas attackDatas;
    [SerializeField] protected string targetTag = "Enemy";

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            foreach (AttackData attack in attackDatas.attackDatas)
            {
                AttackManager.NormalAttack(other.GetComponent<Character>(), attack);
            }
        }
    }
}
