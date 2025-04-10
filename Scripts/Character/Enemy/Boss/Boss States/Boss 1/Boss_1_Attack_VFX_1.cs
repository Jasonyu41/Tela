using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1_Attack_VFX_1 : Boss_1_Attack_VFX
{
    [SerializeField] float sharpness = 5;
    Vector3 originalScale;
    protected override void Awake()
    {
        base.Awake();

        originalScale = tf.localScale;
        tf.localScale = Vector3.zero;
    }

    protected override void OnEnable()
    {
        tf.localScale = Vector3.zero;

        base.OnEnable();
    }

    void Update()
    {
        tf.localScale = Vector3.Slerp(tf.localScale, originalScale, 1 - Mathf.Exp(-sharpness * Time.deltaTime));
    }
}
