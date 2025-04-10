using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1_Attack_VFX_6 : Boss_1_Attack_VFX
{
    [SerializeField] MeshRenderer scan;
    [SerializeField] MeshRenderer dissolve;
    [SerializeField] float minEdge;
    [SerializeField] float maxEdge;
    [SerializeField] float scanTime;
    [SerializeField] float dissolveTime;

    Material mScan;
    Material mDissolve;
    float scanTimer;
    float dissolveTimer;
    int mScanEdgeID = Shader.PropertyToID("_Edge");
    int mDissolveDissolveID = Shader.PropertyToID("_Dissolve");

    protected override void OnEnable()
    {
        base.OnEnable();

        scan.gameObject.SetActive(true);
        dissolve.gameObject.SetActive(false);
        mScan = scan.material;
        mDissolve = dissolve.material;
        mScan.SetFloat(mScanEdgeID, minEdge);
        mDissolve.SetFloat(mDissolveDissolveID, 1f);

        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine()
    {
        scanTimer = 0;

        while (scanTimer < 1)
        {
            scanTimer += Time.deltaTime / scanTime;

            mScan.SetFloat(mScanEdgeID, Mathf.Lerp(minEdge, maxEdge, scanTimer));

            yield return null;
        }

        scan.gameObject.SetActive(false);
        dissolve.gameObject.SetActive(true);

        dissolveTimer = 0;

        while (dissolveTimer < 1)
        {
            dissolveTimer += Time.deltaTime / dissolveTime;

            mDissolve.SetFloat(mDissolveDissolveID, 1f - dissolveTimer);

            yield return null;
        }
    }
}
