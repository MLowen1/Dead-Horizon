using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnControlButton()
    {
        SceneManager.LoadScene(2);
    }

    public void OnPCControlsButton()
    {
        SceneManager.LoadScene(3);
    }

    public void OnArcadeMachineControlsButton()
    {
        SceneManager.LoadScene(4);
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(1);
        GameManager.instance.RestartGame();
    }

    public void OnMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnHowToPlay()
    {
        SceneManager.LoadScene(6);
    }
    
    public void OnQuitButton()
    {
        Application.Quit();
    }
}
