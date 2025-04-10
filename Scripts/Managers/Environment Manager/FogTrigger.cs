using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    [SerializeField] FogSetting fogSetting;
    [SerializeField] float changeTime = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnvironmentManager.Instance.SwitchFogSetting(fogSetting, changeTime);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Test Fog Setting")]
    public void TestFogSetting()
    {
        EnvironmentManager.Instance.TestFogSetting(fogSetting, false);
    }

    [ContextMenu("Clean Fog Setting")]
    public void CleanFogSetting()
    {
        EnvironmentManager.Instance.TestFogSetting(fogSetting, true);
    }
#endif
}

[System.Serializable]
public struct FogSetting
{
    public bool enable;
    public FogMode mode;
    public Color color;
    public float density;
    public float start;
    public float end;
}