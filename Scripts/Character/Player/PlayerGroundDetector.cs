using UnityEngine;

public class PlayerGroundDetector : MonoBehaviour
{
    [SerializeField] float detectionRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;

    Transform tf;
    Collider[] colliders = new Collider[1];
    public bool hasGrounded => Physics.OverlapSphereNonAlloc(tf.position, detectionRadius, colliders, groundLayer) != 0;

    private void Awake()
    {
        tf = transform;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
