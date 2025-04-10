using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatsBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;
    [SerializeField] bool isDelayFill = true;
    [SerializeField] float fillDelay = 0.5f;
    [SerializeField] float fillSpeed = 0.1f;

    float currentFillAmount;
    float targetFillAmount;
    float t;

    WaitForSeconds waitForDelayFill;
    Coroutine bufferedFillingCoroutine;

    private void Awake()
    {
        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    public void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        // targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }

    public void UpdateStats(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue / maxValue;

        if (bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }

        if (currentFillAmount > targetFillAmount)
        {
            fillImageFront.fillAmount = targetFillAmount;
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        }

        if (currentFillAmount < targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }
    }

    IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (isDelayFill)
        {
            yield return waitForDelayFill;
        }

        t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;

            yield return null;
        }
    }
}
