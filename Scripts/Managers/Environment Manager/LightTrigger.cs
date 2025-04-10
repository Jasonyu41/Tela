using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    [SerializeField] LightSetting lightSetting;
    [SerializeField] float changeTime = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnvironmentManager.Instance.SwitchLightSetting(lightSetting, changeTime);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Test Light Setting")]
    public void TestLightSetting()
    {
        EnvironmentManager.Instance.TestLightSetting(lightSetting, false);
    }

    [ContextMenu("Clean Light Setting")]
    public void CleanLightSetting()
    {
        EnvironmentManager.Instance.TestLightSetting(lightSetting, true);
    }
#endif
}

[System.Serializable]
public struct LightSetting
{
    public Color color;
    public float intensity;
    public float indirectMultplier;
    public int index;
}