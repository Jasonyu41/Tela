using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AttackRange : MonoBehaviour
{
    [SerializeField] GameObject attackVFX;
    [SerializeField] float waitStartTranslationTime;
    [SerializeField] float translationTime = 1f;
    [SerializeField] float generateTime = 1f;
    [SerializeField] bool isRectangle = false;
    [SerializeField] AudioData[] audioDatas;
    
    DecalProjector decalProjector_Original;
    DecalProjector decalProjector_Translation;
    Vector3 targetSize;
    Vector3 targetPivot;
    float addOpacity;
    float addSizeX;
    float addSizeY;
    float projectionDepth;
    float startTranslationTimer;
    float generateTimer;
    bool isTranslationFinished;
    bool isGenerateFinished;
    WaitForFixedUpdate waitForFixedUpdate;

    void Awake()
    {
        decalProjector_Original = transform.GetChild(0).GetComponent<DecalProjector>();
        decalProjector_Translation = transform.GetChild(1).GetComponent<DecalProjector>();

        addOpacity = 1f / translationTime;
        addSizeX = decalProjector_Original.size.x / translationTime;
        addSizeY = decalProjector_Original.size.y / translationTime;
        projectionDepth = decalProjector_Translation.size.z;

        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    void OnEnable()
    {
        decalProjector_Original.fadeFactor = 0.7f;
        decalProjector_Translation.size = Vector3.zero;
        targetSize = Vector3.zero;
        targetSize.z = projectionDepth;
        targetPivot= Vector3.zero;
        isTranslationFinished = false;
        isGenerateFinished = false;

        StartCoroutine(Translation());
        StartCoroutine(Generate());
    }

    IEnumerator Translation()
    {
        startTranslationTimer = 0;
        while (startTranslationTimer < waitStartTranslationTime)
        {
            startTranslationTimer += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }

        while (decalProjector_Translation.size.y < decalProjector_Original.size.y)
        {
            targetSize.x += addSizeX * Time.fixedDeltaTime;
            targetSize.y += addSizeY * Time.fixedDeltaTime;
            decalProjector_Translation.size = targetSize;

            if (decalProjector_Translation.size.y * 2.5f >= decalProjector_Original.size.y)
            {
                decalProjector_Original.fadeFactor -= addOpacity * Time.fixedDeltaTime;
            }

            if (isRectangle)
            {
                targetPivot = decalProjector_Translation.pivot;
                targetPivot.y = targetSize.y * 0.5f;
                decalProjector_Translation.pivot = targetPivot;
            }

            yield return waitForFixedUpdate;
        }

        isTranslationFinished = true;
        Finished();
    }

    IEnumerator Generate()
    {
        generateTimer = 0;
        while (generateTimer < generateTime)
        {
            generateTimer += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }

        yield return null;
        PoolManager.Release(attackVFX, transform.position, transform.rotation);

        isGenerateFinished = true;
        Finished();
    }

    void Finished()
    {
        if (isGenerateFinished && isTranslationFinished)
        {
            if (audioDatas.Length >= 1)
            {
                AudioManager.Instance.PlaySFX(audioDatas);
            }
            gameObject.SetActive(false);
        }
    }
}
