using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HDRIRotate : MonoBehaviour
{
    [SerializeField] float hDRIRotationSpeedMin;
    [SerializeField] float hDRIRotationSpeedMax;
    Material hDRIMaterial;
    float hDRIRotation;
    int hDRIRotationID;

    private void Awake()
    {
        hDRIMaterial = RenderSettings.skybox;
        hDRIRotation = 0f;
        hDRIRotationID = Shader.PropertyToID("_Rotation");
    }

    void Update()
    {
        hDRIRotation += Random.Range(hDRIRotationSpeedMin, hDRIRotationSpeedMax) * Time.deltaTime;
        if (hDRIRotation >= 360f) hDRIRotation -= 360f;
        hDRIMaterial.SetFloat(hDRIRotationID, hDRIRotation);
    }
}
