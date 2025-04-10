using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("---- Character Base Seting ----")]
    [SerializeField] public CharacterData characterData;
    [SerializeField] public List<Buff> buffsList = new List<Buff>();                        //HideInInspector
    [SerializeField] public List<string> buffsNameList = new List<string>();                //HideInInspector
    [SerializeField] public StatusEffect statusEffect;                                  //HideInInspector

    [SerializeField] bool isDistroyThisObject;


    [Header("---- Animation ----")]
    [SerializeField] string takeDamegeAnimation;
    // [SerializeField] string beforeDieAnimation;
    [SerializeField] string dieAnimation;
    protected Animator animator;


    [Header("---- VFX ----")]
    [SerializeField] GameObject takeDamege_VFX;
    [SerializeField] GameObject beforeDie_VFX;
    [SerializeField] GameObject die_VFX;


    protected Transform tf;


    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        tf = transform;

        characterData.health = characterData.maxHealth;
    }

    protected virtual void OnEnable()
    {
        // characterData.health = characterData.maxHealth;
    }

    protected virtual void Update()
    {
        BuffUpdate();
    }

    void BuffUpdate()
    {
        if (buffsList.Count() == 0) return;
        
        for (int i = 0; i < buffsList.Count; i++)
        {
            buffsList[i].LogicUpdate();
        }
    }

    public virtual void TakeDamege(float value)
    {
        if (characterData.health <= 0) return;
        
        characterData.health -= value;

        if (characterData.health > 0)
        {
            if (takeDamegeAnimation != "")
            {
                animator.Play(takeDamegeAnimation);
            }

            if (takeDamege_VFX != null)
            {
                PoolManager.Release(takeDamege_VFX, tf.position);
            }
        }
        else
        {
            BeforeDie();
        }
    }

    public virtual void BeforeDie()
    {
        GetComponent<Collider>().enabled = false;

        if (dieAnimation != "")
        {
            StartCoroutine(DieAnimation());
        }
        else
        {
            Die();
        }

        if (beforeDie_VFX != null)
        {
            PoolManager.Release(beforeDie_VFX, tf.position);
        }
    }

    IEnumerator DieAnimation()
    {
        animator.Play(dieAnimation);

        yield return null;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f);

        Die();
    }

    public virtual void Die()
    {
        characterData.health = 0;

        if (die_VFX != null)
        {
            PoolManager.Release(die_VFX, tf.position);
        }
        
        if (isDistroyThisObject)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void RestoreHealth(float value)
    {
        characterData.health = Mathf.Clamp(characterData.health += value, 0f, characterData.maxHealth);
    }

    public void AddAnimationSpeed(float value)
    {
        animator.speed += value;
    }
}
