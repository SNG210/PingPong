using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void OnResumeButtonClicked()
    {
        GameManager.Instance.TogglePauseMenu();
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
