using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HDRITrigger : MonoBehaviour
{
    [SerializeField] HDRISetting hDRISetting;
    [SerializeField] float changeTime = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnvironmentManager.Instance.SwitchHDRISetting(hDRISetting, changeTime);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Test HDRI Setting")]
    public void TestHDRISetting()
    {
        EnvironmentManager.Instance.TestHDRISetting(hDRISetting, false);
    }

    [ContextMenu("Clean HDRI Setting")]
    public void CleanHDRISetting()
    {
        EnvironmentManager.Instance.TestHDRISetting(hDRISetting, true);
    }
#endif
}

[System.Serializable]
public struct HDRISetting
{
    public Color color;
    public float exposure;
    public Texture targetHDRI;
}