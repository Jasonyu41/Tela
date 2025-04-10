using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeFogTrigger : MonoBehaviour
{
    [SerializeField] VolumeFogSetting volumeFogSetting;
    [SerializeField] float changeTime = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnvironmentManager.Instance.SwitchVolumeFogSetting(volumeFogSetting, changeTime);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Test VolumeFog Setting")]
    public void TestVolumeFogSetting()
    {
        EnvironmentManager.Instance.TestVolumeFogSetting(volumeFogSetting, false);
    }

    [ContextMenu("Clean VolumeFog Setting")]
    public void CleanVolumeFogSetting()
    {
        EnvironmentManager.Instance.TestVolumeFogSetting(volumeFogSetting, true);
    }
#endif
}

[System.Serializable]
public struct VolumeFogSetting
{
    public Color color;
    public float Speed;
    public float Size;
    public float Threshold;
    public float Multplier;
}