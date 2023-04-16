using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayRPS()
    {
        SceneManager.LoadScene("RPS_Scene");
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
