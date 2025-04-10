using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentLightingTrigger : MonoBehaviour
{
    [SerializeField] EnviromentLightingSetting enviromentLightingSetting;
    [SerializeField] float changeTime = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnvironmentManager.Instance.SwitchEnviromentLightingSetting(enviromentLightingSetting, changeTime);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Test EnviromentLighting Setting")]
    public void TestEnviromentLightingSetting()
    {
        EnvironmentManager.Instance.TestEnviromentLightingSetting(enviromentLightingSetting, false);
    }

    [ContextMenu("Clean EnviromentLighting Setting")]
    public void CleanEnviromentLightingSetting()
    {
        EnvironmentManager.Instance.TestEnviromentLightingSetting(enviromentLightingSetting, true);
    }
#endif
}

[System.Serializable]
public struct EnviromentLightingSetting
{
    [ColorUsage(true, true)] public Color skyColor;
    [ColorUsage(true, true)] public Color equatorColor;
    [ColorUsage(true, true)] public Color groundColor;
}
