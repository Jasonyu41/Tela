using UnityEngine;
using TMPro;

public class AreaTitleTrigger : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        var canvasGroup = text.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        // Color tempColor = text.color;
        // tempColor.a = 0;
        // text.color = tempColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnvironmentManager.Instance.ShowAreaTitle(text);
        }
    }
}

[System.Serializable]
public struct AreaTitleSetting
{
    public float enableTime;
    public float showTime;
    public float disableTime;
}