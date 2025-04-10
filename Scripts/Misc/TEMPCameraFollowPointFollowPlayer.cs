using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPCameraFollowPointFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    
    Transform tf;

    static readonly int CACHE_SIZE = 3;
    Vector3[] velCache = new Vector3[CACHE_SIZE];
    int currentChacheIndex = 0;

    private void Awake()
    {
        tf = transform;
    }

    private void FixedUpdate()
    {
        tf.position = AverageVel(player.position);
    }

    Vector3 AverageVel(Vector3 newVel)
    {
        velCache[currentChacheIndex] = newVel;
        currentChacheIndex++;
        currentChacheIndex %= CACHE_SIZE;
        Vector3 average = Vector3.zero;
        foreach (Vector3 vel in velCache)
        {
            average += vel;
        }
        return average / CACHE_SIZE;
    }
}
