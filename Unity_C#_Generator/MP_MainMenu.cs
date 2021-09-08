using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MP_MainMenu : MonoBehaviour
{
    public void LoadScreen(int i)
    {
        SceneManager.LoadScene(i, LoadSceneMode.Single);

    }
    public void QuitApp()
    {
        Application.Quit();
    }

}
