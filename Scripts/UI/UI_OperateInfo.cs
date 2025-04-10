using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UI_OperateInfo : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Image background;
    [SerializeField] RawImage rawImage;
    [SerializeField] VideoPlayer skillVideo;
    [SerializeField] Image skillBackground;

    private void OnEnable()
    {
        playerInput.onPause += OnCancel;
        playerInput.onCancel += OnCancel;

        GameManager.Instance.Pause(false);
        OpenSkillInfo();
    }

    private void OnDisable()
    {
        playerInput.onPause -= OnCancel;
        playerInput.onCancel -= OnCancel;
    }

    private void OnCancel()
    {
        gameObject.SetActive(false);
        GameManager.Instance.UnPause();
    }

    private void OpenSkillInfo()
    {
        background.enabled = false;
        rawImage.enabled = true;
        skillVideo.gameObject.SetActive(true);
        skillBackground.enabled = true;
    }
}
