using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_Pause : MonoBehaviour
{
    [SerializeField] string mainMenuName = "Main Menu";
    [SerializeField] PlayerInput playerInput;
    [SerializeField] GameObject packageButton;
    [SerializeField] UI_Package UI_Package;
    [SerializeField] GameObject pauseExitInfo;
    [SerializeField] GameObject pauseExitNoButton;

    private void Awake()
    {
        pauseExitInfo.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerInput.onCancel += OnCancel;
        EventSystem.current.SetSelectedGameObject(packageButton);
    }

    private void OnDisable()
    {
        playerInput.onCancel -= OnCancel;
        gameObject.SetActive(false);
    }

    private void OnCancel()
    {
        if (pauseExitInfo.activeSelf)
        {
            pauseExitInfo.SetActive(false);
        }
        else
        {
            GameManager.Instance.UnPause();
        }
    }

    public void OpenPackageUI()
    {
        UI_Package.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OpenPartyUI()
    {
        // UI_Party.gameObject.SetActibe(true);
        gameObject.SetActive(false);
    }

    public void OpenSettingUI()
    {
        // UI_Setting.gameObject.SetActibe(true);
        gameObject.SetActive(false);
    }

    public void OpenLoadGameUI()
    {
        // UI_LoadGame.gameObject.SetActibe(true);
        gameObject.SetActive(false);
    }

    public void OpenPauseExitInfo()
    {
        pauseExitInfo.SetActive(true);
        EventSystem.current.SetSelectedGameObject(pauseExitNoButton);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuName);
    }
}
