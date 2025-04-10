using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
