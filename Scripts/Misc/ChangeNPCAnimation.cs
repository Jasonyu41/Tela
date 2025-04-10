using System.Collections;
using UnityEngine;

public class ChangeNPCAnimation : MonoBehaviour
{
    [SerializeField] float minChangeAnimationTime = 40f;
    [SerializeField] float maxChangeAnimationTime = 60f;

    Animator animator;
    int triggerID;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        triggerID = Animator.StringToHash("ChangeAnimation");
    }

    private void Start()
    {
        StartCoroutine(ChangeAnimation());
    }
    
    IEnumerator ChangeAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minChangeAnimationTime, maxChangeAnimationTime));

            animator.SetTrigger(triggerID);
        }
    }
}
