using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1_Attack_VFX_2 : Boss_1_Attack_VFX
{
    [SerializeField] float maxRotateSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 targetOffset;
    Transform targetTransform;

    protected override void OnEnable()
    {
        base.OnEnable();

        targetTransform = GameObject.FindGameObjectWithTag(targetTag).transform;
    }

    void Update()
    {
        tf.rotation = Quaternion.LookRotation(Vector3.RotateTowards(tf.forward, targetTransform.TransformPoint(targetOffset) - tf.position, maxRotateSpeed * Time.deltaTime, 0));
        tf.position += tf.forward * moveSpeed * Time.deltaTime;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.tag == targetTag)
        {
            gameObject.SetActive(false);
        }
    }
}
