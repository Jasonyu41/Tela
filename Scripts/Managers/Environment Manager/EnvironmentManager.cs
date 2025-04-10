using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnvironmentManager : Singleton<EnvironmentManager>
{
    [SerializeField] bool isUseDefaultFog = true;
    [SerializeField] bool isUseDefaultVolumeFog = true;
    [SerializeField] bool isUseDefaultHDRI = true;
    [SerializeField] bool isUseDefaultLight = true;
    [SerializeField] bool isUseDefaultEnviromentLighting = true;


    [SerializeField] Light[] lights;

    [SerializeField] FogSetting defaultFog;
    [SerializeField] VolumeFogSetting defaultVolumeFog;
    [SerializeField] Material volumeFogMaterial;
    [SerializeField] HDRISetting defaultHDRI;
    [SerializeField] float hDRIRotationSpeedMin;
    [SerializeField] float hDRIRotationSpeedMax;
    [SerializeField] public LightSetting defaultLight;
    [SerializeField] EnviromentLightingSetting defaultEnviromentLighting;
    [SerializeField] AreaTitleSetting areaTitle;

    #region Fog Variable
    Color fogOriginalColor;
    float fogOriginalDensity;
    float fogOriginalStart;
    float fogOriginalEnd;
    float fogTimer;
    Coroutine lerpFogCoroutine;
    #endregion
    
    #region Volume Fog
    Color volumeFogOriginalColor;
    float volumeFogOriginalSpeed;
    float volumeFogOriginalMultiplier;
    float volumeFogOriginalThreshold;
    float volumeFogOriginalSize;
    int volumeFogColorID;
    int volumeFogSpeedID;
    int volumeFogMultiplierID;
    int volumeFogThresholdID;
    int volumeFogSizeID;
    [SerializeField] float volumeFogSpeed;
    float volumeFogMove;
    float volumeFogTimer;
    Coroutine lerpVolumeFogCoroutine;
    #endregion

    #region HDRI Variable
    Material hDRIMaterial;
    Color hDRIOriginalColor;
    float hDRIOriginalExposure;
    float hDRIRotation = 0f;
    float hDRIOriginalBlend;
    int hDRITintID;
    int hDRIExposureID;
    int hDRIRotationID;
    int hDRIBlendID;
    int hDRITexID;
    int hDRITex2ID;
    float hDRITimer;
    Coroutine lerpHDRICoroutine;
    #endregion

    #region Light Variable
    Color lightOriginalColor;
    float lightOriginalIntensity;
    float lightOriginalIndirectMultplier;
    float lightTimer;
    Coroutine lerpLightCoroutine;
    #endregion

    #region Enviroment Lighting
    Color enviromentLightingOriginalSkyColor;
    Color enviromentLightingOriginalEquatorColor;
    Color enviromentLightingOriginalGroundColor;
    float enviromentLightingTimer;
    Coroutine enviromentLightingCoroutine;
    #endregion

    #region Area Title
    List<TextMeshProUGUI> text = new List<TextMeshProUGUI>();
    float areaTitleTimer;
    WaitForSeconds waitForShowTime;
    #endregion


    protected override void Awake()
    {
        base.Awake();

        waitForShowTime = new WaitForSeconds(areaTitle.showTime);
        hDRIMaterial = RenderSettings.skybox;

        volumeFogColorID = Shader.PropertyToID("_Color");
        volumeFogSpeedID = Shader.PropertyToID("_Speed");
        volumeFogMultiplierID = Shader.PropertyToID("_Multiplier");
        volumeFogThresholdID = Shader.PropertyToID("_Threshold");
        volumeFogSizeID = Shader.PropertyToID("_Size");

        hDRITintID = Shader.PropertyToID("_Tint");
        hDRIExposureID = Shader.PropertyToID("_Exposure");
        hDRIRotationID = Shader.PropertyToID("_Rotation");
        hDRIBlendID = Shader.PropertyToID("_Blend");
        hDRITexID = Shader.PropertyToID("_Tex");
        hDRITex2ID = Shader.PropertyToID("_Tex2");
    }

    private void Start()
    {
        if (isUseDefaultFog)
        {
            RenderSettings.fog = defaultFog.enable;
            RenderSettings.fogMode = defaultFog.mode;
            RenderSettings.fogColor = defaultFog.color;
            RenderSettings.fogDensity = defaultFog.density;
            RenderSettings.fogStartDistance = defaultFog.start;
            RenderSettings.fogEndDistance = defaultFog.end;
        }

        if (isUseDefaultVolumeFog)
        {
            volumeFogMaterial.SetColor(volumeFogColorID, defaultVolumeFog.color);
            volumeFogMaterial.SetFloat(volumeFogSpeedID, 0);
            volumeFogSpeed = defaultVolumeFog.Speed;
            volumeFogMaterial.SetFloat(volumeFogMultiplierID, defaultVolumeFog.Multplier);
            volumeFogMaterial.SetFloat(volumeFogThresholdID, defaultVolumeFog.Threshold);
            volumeFogMaterial.SetFloat(volumeFogSizeID, defaultVolumeFog.Size);
        }

        if (isUseDefaultHDRI)
        {
            hDRIMaterial.SetColor(hDRITintID, defaultHDRI.color);
            hDRIMaterial.SetFloat(hDRIExposureID, defaultHDRI.exposure);
            hDRIMaterial.SetFloat(hDRIRotationID, hDRIRotation);
            hDRIMaterial.SetFloat(hDRIBlendID, 0);
            hDRIMaterial.SetTexture(hDRITexID, defaultHDRI.targetHDRI);
        }

        if (isUseDefaultLight)
        {
            lights[defaultLight.index].color = defaultLight.color;
            lights[defaultLight.index].intensity = defaultLight.intensity;
            lights[defaultLight.index].bounceIntensity = defaultLight.indirectMultplier;
        }

        if (isUseDefaultEnviromentLighting)
        {
            RenderSettings.ambientSkyColor = defaultEnviromentLighting.skyColor;
            RenderSettings.ambientEquatorColor = defaultEnviromentLighting.equatorColor;
            RenderSettings.ambientGroundColor = defaultEnviromentLighting.groundColor;
        }
    }

#if UNITY_EDITOR
    private void OnDisable()
    {
        TestFogSetting(defaultFog, true);
        TestHDRISetting(defaultHDRI, true);
        volumeFogMaterial.SetFloat(volumeFogSpeedID, 0);
        hDRIMaterial.SetFloat(hDRIRotationID, 0);
    }
#endif

    private void Update()
    {
        volumeFogMove += volumeFogSpeed * Time.deltaTime;
        volumeFogMaterial.SetFloat(volumeFogSpeedID, volumeFogMove);

        hDRIRotation += Random.Range(hDRIRotationSpeedMin, hDRIRotationSpeedMax) * Time.deltaTime;
        if (hDRIRotation >= 360f) hDRIRotation -= 360f;
        hDRIMaterial.SetFloat(hDRIRotationID, hDRIRotation);
    }

    public void SwitchFogSetting(FogSetting fogSetting, float changeTime)
    {
        RenderSettings.fog = fogSetting.enable;
        RenderSettings.fogMode = fogSetting.mode;

        if (lerpFogCoroutine != null)
        {
            StopCoroutine(lerpFogCoroutine);
        }
        lerpFogCoroutine = StartCoroutine(LerpFogCoroutine(fogSetting, changeTime));
    }
    IEnumerator LerpFogCoroutine(FogSetting fogSetting, float changeTime)
    {
        fogTimer = 0;
        fogOriginalColor = RenderSettings.fogColor;
        fogOriginalDensity = RenderSettings.fogDensity;
        fogOriginalStart = RenderSettings.fogStartDistance;
        fogOriginalEnd = RenderSettings.fogEndDistance;

        while (fogTimer < 1)
        {
            fogTimer += Time.deltaTime / changeTime;

            RenderSettings.fogColor = Color.Lerp(fogOriginalColor, fogSetting.color, fogTimer);
            RenderSettings.fogDensity = Mathf.Lerp(fogOriginalDensity, fogSetting.density, fogTimer);
            RenderSettings.fogStartDistance = Mathf.Lerp(fogOriginalStart, fogSetting.start, fogTimer);
            RenderSettings.fogEndDistance = Mathf.Lerp(fogOriginalEnd, fogSetting.end, fogTimer);

            yield return null;
        }
    }
    
    public void SwitchVolumeFogSetting(VolumeFogSetting volumeFogSetting, float changeTime)
    {
        if (lerpVolumeFogCoroutine != null)
        {
            StopCoroutine(lerpVolumeFogCoroutine);
        }
        lerpVolumeFogCoroutine = StartCoroutine(LerpVolumeFogCoroutine(volumeFogSetting, changeTime));
    }
    IEnumerator LerpVolumeFogCoroutine(VolumeFogSetting volumeFogSetting, float changeTime)
    {
        volumeFogTimer = 0;
        volumeFogOriginalColor = volumeFogMaterial.GetColor(volumeFogColorID);
        volumeFogOriginalSpeed = volumeFogSpeed;
        volumeFogOriginalMultiplier = volumeFogMaterial.GetFloat(volumeFogMultiplierID);
        volumeFogOriginalThreshold = volumeFogMaterial.GetFloat(volumeFogThresholdID);
        volumeFogOriginalSize = volumeFogMaterial.GetFloat(volumeFogSizeID);

        while (volumeFogTimer < 1)
        {
            volumeFogTimer += Time.deltaTime / changeTime;

            volumeFogMaterial.SetColor(volumeFogColorID, Color.Lerp(volumeFogOriginalColor, volumeFogSetting.color, volumeFogTimer));
            volumeFogSpeed = Mathf.Lerp(volumeFogOriginalSpeed, volumeFogSetting.Speed, volumeFogTimer);
            volumeFogMaterial.SetFloat(volumeFogMultiplierID, Mathf.Lerp(volumeFogOriginalMultiplier, volumeFogSetting.Multplier, volumeFogTimer));
            volumeFogMaterial.SetFloat(volumeFogThresholdID, Mathf.Lerp(volumeFogOriginalThreshold, volumeFogSetting.Threshold, volumeFogTimer));
            volumeFogMaterial.SetFloat(volumeFogSizeID, Mathf.Lerp(volumeFogOriginalSize, volumeFogSetting.Size, volumeFogTimer));

            yield return null;
        }
    }

    public void SwitchHDRISetting(HDRISetting hDRISetting, float changeTime)
    {
        if (lerpHDRICoroutine != null)
        {
            StopCoroutine(lerpHDRICoroutine);
        }
        lerpHDRICoroutine = StartCoroutine(LerpHDRICoroutine(hDRISetting, changeTime));
    }
    IEnumerator LerpHDRICoroutine(HDRISetting hDRISetting, float changeTime)
    {
        hDRITimer = 0;
        hDRIOriginalColor = hDRIMaterial.GetColor(hDRITintID);
        hDRIOriginalExposure = hDRIMaterial.GetFloat(hDRIExposureID);
        hDRIOriginalBlend = 1 - hDRIMaterial.GetFloat(hDRIBlendID);
        hDRIMaterial.SetFloat(hDRIBlendID, hDRIOriginalBlend);
        hDRIMaterial.SetTexture(hDRITexID, hDRIMaterial.GetTexture(hDRITex2ID));
        hDRIMaterial.SetTexture(hDRITex2ID, hDRISetting.targetHDRI);

        while (hDRITimer < 1)
        {
            hDRITimer += Time.deltaTime / changeTime;

            hDRIMaterial.SetColor(hDRITintID, Color.Lerp(hDRIOriginalColor, hDRISetting.color, hDRITimer));
            hDRIMaterial.SetFloat(hDRIExposureID, Mathf.Lerp(hDRIOriginalExposure, hDRISetting.exposure, hDRITimer));
            hDRIMaterial.SetFloat(hDRIBlendID, Mathf.Lerp(hDRIOriginalBlend, 1, hDRITimer));

            yield return null;
        }
    }

    public void SwitchLightSetting(LightSetting lightSetting, float changeTime)
    {
        if (lerpLightCoroutine != null)
        {
            StopCoroutine(lerpLightCoroutine);
        }
        lerpLightCoroutine = StartCoroutine(LerpLightCoroutine(lightSetting, changeTime));
    }
    IEnumerator LerpLightCoroutine(LightSetting lightSetting, float changeTime)
    {
        lightTimer = 0;
        lightOriginalColor = lights[lightSetting.index].color;
        lightOriginalIntensity = lights[lightSetting.index].intensity;
        lightOriginalIndirectMultplier = lights[lightSetting.index].bounceIntensity;

        while (lightTimer < 1)
        {
            lightTimer += Time.deltaTime / changeTime;

            lights[lightSetting.index].color = Color.Lerp(lightOriginalColor, lightSetting.color, lightTimer);
            lights[lightSetting.index].intensity = Mathf.Lerp(lightOriginalIntensity, lightSetting.intensity, lightTimer);
            lights[lightSetting.index].bounceIntensity = Mathf.Lerp(lightOriginalIndirectMultplier, lightSetting.indirectMultplier, lightTimer);

            yield return null;
        }
    }

    public void SwitchEnviromentLightingSetting(EnviromentLightingSetting enviromentLightingSetting, float changeTime)
    {
        if (enviromentLightingCoroutine != null)
        {
            StopCoroutine(enviromentLightingCoroutine);
        }
        enviromentLightingCoroutine = StartCoroutine(LerpEnviromentLightingCoroutine(enviromentLightingSetting, changeTime));
    }
    IEnumerator LerpEnviromentLightingCoroutine(EnviromentLightingSetting enviromentLightingSetting, float changeTime)
    {
        enviromentLightingTimer = 0;
        enviromentLightingOriginalSkyColor = RenderSettings.ambientSkyColor;
        enviromentLightingOriginalEquatorColor = RenderSettings.ambientEquatorColor;
        enviromentLightingOriginalGroundColor = RenderSettings.ambientGroundColor;

        while (enviromentLightingTimer < 1)
        {
            enviromentLightingTimer += Time.deltaTime / changeTime;

            RenderSettings.ambientSkyColor = Color.Lerp(enviromentLightingOriginalSkyColor, enviromentLightingSetting.skyColor, enviromentLightingTimer);
            RenderSettings.ambientEquatorColor = Color.Lerp(enviromentLightingOriginalEquatorColor, enviromentLightingSetting.equatorColor, enviromentLightingTimer);
            RenderSettings.ambientGroundColor = Color.Lerp(enviromentLightingOriginalGroundColor, enviromentLightingSetting.groundColor, enviromentLightingTimer);

            yield return null;
        }
    }

    public void ShowAreaTitle(TextMeshProUGUI text)
    {
        if (!this.text.Contains(text))
        {
            this.text.Add(text);
            if (this.text.Count == 1)
            {
                StartCoroutine(LerpAreaTitleCoroutine());
            }
        }
    }
    IEnumerator LerpAreaTitleCoroutine()
    {
        while (text.Count > 0)
        {
            areaTitleTimer = 0;
            
            // Color color = text[0].color;
            var canvasGroup = text[0].GetComponent<CanvasGroup>();
            while (areaTitleTimer < 1)
            {
                areaTitleTimer += Time.deltaTime / areaTitle.enableTime;

                canvasGroup.alpha = areaTitleTimer;
                // color.a = areaTitleTimer;
                // text[0].color = color;

                yield return null;
            }
            
            yield return waitForShowTime;

            while (areaTitleTimer > 0)
            {
                areaTitleTimer -= Time.deltaTime / areaTitle.disableTime;

                canvasGroup.alpha = areaTitleTimer;
                // color.a = areaTitleTimer;
                // text[0].color = color;

                yield return null;
            }

            text.RemoveAt(0);
        }
    }

#if UNITY_EDITOR
    public void TestFogSetting(FogSetting fogSetting, bool isUseDefault)
    {
        if (isUseDefault) fogSetting = defaultFog;

        RenderSettings.fog = fogSetting.enable;
        RenderSettings.fogMode = fogSetting.mode;
        RenderSettings.fogColor = fogSetting.color;
        RenderSettings.fogDensity = fogSetting.density;
        RenderSettings.fogStartDistance = fogSetting.start;
        RenderSettings.fogEndDistance = fogSetting.end;
    }

    public void TestVolumeFogSetting(VolumeFogSetting volumeFogSetting, bool isUseDefault)
    {
        if (isUseDefault) volumeFogSetting = defaultVolumeFog;

        volumeFogMaterial.SetColor(volumeFogColorID, volumeFogSetting.color);
        volumeFogMaterial.SetFloat(volumeFogMultiplierID, volumeFogSetting.Multplier);
        volumeFogMaterial.SetFloat(volumeFogThresholdID, volumeFogSetting.Threshold);
        volumeFogMaterial.SetFloat(volumeFogSizeID, volumeFogSetting.Size);
    }

    public void TestHDRISetting(HDRISetting hDRISetting, bool isUseDefault)
    {
        if (isUseDefault) hDRISetting = defaultHDRI;

        hDRIMaterial.SetColor(hDRITintID, hDRISetting.color);
        hDRIMaterial.SetFloat(hDRIExposureID, hDRISetting.exposure);
    }

    public void TestLightSetting(LightSetting lightSetting, bool isUseDefault)
    {
        if (isUseDefault) lightSetting = defaultLight;

        lights[lightSetting.index].color = lightSetting.color;
        lights[lightSetting.index].intensity = lightSetting.intensity;
        lights[lightSetting.index].bounceIntensity = lightSetting.indirectMultplier;
    }

    public void TestEnviromentLightingSetting(EnviromentLightingSetting enviromentLightingSetting, bool isUseDefault)
    {
        if (isUseDefault) enviromentLightingSetting = defaultEnviromentLighting;

        RenderSettings.ambientSkyColor = enviromentLightingSetting.skyColor;
        RenderSettings.ambientEquatorColor = enviromentLightingSetting.equatorColor;
        RenderSettings.ambientGroundColor = enviromentLightingSetting.groundColor;
    }
#endif
}