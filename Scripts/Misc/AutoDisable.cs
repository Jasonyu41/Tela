using System.Collections;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    [SerializeField] float autoDisableTime;
    [SerializeField] bool isDestory = false;

    WaitForSeconds waitForAutoDisableTime;

    void Awake()
    {
        waitForAutoDisableTime = new WaitForSeconds(autoDisableTime);
    }

    void OnEnable()
    {
        StartCoroutine(TimingDisable());
    }

    IEnumerator TimingDisable()
    {
        yield return waitForAutoDisableTime;

        if (isDestory)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
