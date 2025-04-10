using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMove : MonoBehaviour
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
            tf.position = target.position;
            elapsedFrame = 0;
        }
    }
}
