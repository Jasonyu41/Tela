using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HookLockTrigger : MonoBehaviour
{
    [SerializeField] Image normalImage;
    [SerializeField] Image selectImage;
    [SerializeField] public Transform highestPoint;
    public bool isInVisualField;

    private void OnEnable()
    {
        ChangeImage(true);
    }

    private void OnBecameVisible()
    {
        isInVisualField = true;
    }
    
    private void OnBecameInvisible()
    {
        isInVisualField = false;
    }

    public void ChangeImage(bool isStart)
    {
        normalImage.enabled = isStart;
        selectImage.enabled = !isStart;
    }
}
