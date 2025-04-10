using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartMovie : MonoBehaviour
{
    [SerializeField] Canvas hdr;
    [SerializeField] RawImage rawImage;
    [SerializeField] Image image;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] PlayableDirector timeline;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] PlayerCharacterController playerCharacterController;
    [SerializeField] float waitVideoPlayerTime;
    [SerializeField] float waitEnableScriptTime;

    Coroutine coroutine;
    float timer;

    public void Play()
    {
        playerCharacterController.enabled = false;
        PlayVideoPlayer();
    }

    public void PlayVideoPlayer()
    {
        hdr.enabled = false;
        image.enabled = true;
        rawImage.enabled = true;
        videoPlayer.Play();
        coroutine = StartCoroutine(VideoPlayer());
        virtualCamera.Priority = 100;
    }

    IEnumerator VideoPlayer()
    {
        yield return new WaitForSeconds(waitVideoPlayerTime);

        PlayTimeline();
    }

    public void PlayTimeline()
    {
        image.enabled = false;
        rawImage.enabled = false;
        timer = 0;
        timeline.Play();
        StartCoroutine(Timing());
    }

    IEnumerator Timing()
    {
        while (timer < waitEnableScriptTime)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        GameManager.Instance.UnPause();
        playerCharacterController.enabled = true;
    }

// #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            videoPlayer.Stop();
            StopCoroutine(coroutine);
            PlayTimeline();
        }
    }
// #endif
}
