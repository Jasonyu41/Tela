using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1_Shield : MonoBehaviour
{
    Camera _cam;
    Renderer _renderer;
    [SerializeField] AnimationCurve _DisplacementCurve;
    [SerializeField] float _DisplacementMagnitude;
    [SerializeField] float _LerpSpeed;
    [SerializeField] float _DisolveSpeed;
    bool _shieldOn;
    Collider _collider;
    Coroutine _disolveCoroutine;

    private void Awake()
    {
        _cam = Camera.main;
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        transform.forward = _cam.transform.position - transform.position;

        Vector3 screenPoint = _cam.WorldToScreenPoint(transform.position);
        screenPoint.x = screenPoint.x / Screen.width;
        screenPoint.y = screenPoint.y / Screen.height;
        _renderer.material.SetVector("_ObjScreenPos", screenPoint);

        // if (Input.GetMouseButtonDown(0))
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     if (Physics.Raycast(ray, out hit))
        //     {
        //         print(hit.point);
        //         HitShield(hit.point);
        //     }
        // }
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     OpenCloseShield();
        // }
    }
[SerializeField] Vector3 testve3;
[ContextMenu("TEST")]
    public void test()
    {
        HitShield(testve3);
    }

    public void HitShield(Vector3 hitPos)
    {
        _renderer.material.SetVector("_HitPos", hitPos);
        StopAllCoroutines();
        StartCoroutine(Coroutine_HitDisplacement());
    }

    public void OpenCloseShield()
    {
        float target = 1;
        if (_shieldOn)
        {
            target = 0;
        }
        _shieldOn = !_shieldOn;
        _collider.enabled = _shieldOn;
        if (_disolveCoroutine != null)
        {
            StopCoroutine(_disolveCoroutine);
        }
        _disolveCoroutine = StartCoroutine(Coroutine_DisolveShield(target));
    }

    IEnumerator Coroutine_HitDisplacement()
    {
        float lerp = 0;
        while (lerp < 1)
        {
            _renderer.material.SetFloat("_DisplacementStrength", _DisplacementCurve.Evaluate(lerp) * _DisplacementMagnitude);
            lerp += Time.deltaTime * _LerpSpeed;
            yield return null;
        }
    }

    IEnumerator Coroutine_DisolveShield(float target)
    {
        float start = _renderer.material.GetFloat("_Disolve");
        float lerp = 0;
        while (lerp < 1)
        {
            _renderer.material.SetFloat("_Disolve", Mathf.Lerp(start, target, lerp));
            lerp += Time.deltaTime * _DisolveSpeed;
            yield return null;
        }
    }
}
