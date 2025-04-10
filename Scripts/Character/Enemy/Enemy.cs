using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [Header("---- Enemy Base Seting ----")]
    [SerializeField] public float touchEnemyDamage;
    // [SerializeField] Drops
    

    public override void Die()
    {
        base.Die();

        // Drops
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (touchEnemyDamage == 0) return;
        
        if (other.transform.tag == "Player")
        {
            other.transform.GetComponent<Character>().TakeDamege(touchEnemyDamage);
        }
    }
}
