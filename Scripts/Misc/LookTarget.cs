using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] int intervalFrame = 3;

    Transform tf;
    int elapsedFrame;

    private void Awake()
    {
        tf = transform;
    }

    void Update()
    {
        elapsedFrame++;
        if (elapsedFrame % intervalFrame == 0)
        {
            tf.LookAt(target);
            elapsedFrame = 0;
        }
    }
}
