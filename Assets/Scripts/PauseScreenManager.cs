using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenManager : MonoBehaviour
{
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject pauseCanvas;
    private bool open;

    public void OpenPauseMenu()
    {
        pauseCanvas.SetActive(true);
        open = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePauseMenu()
    {
        pauseCanvas.SetActive(false);
        open = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !loseScreen.activeInHierarchy) 
        {
            if (open)
                ClosePauseMenu();
            else
                OpenPauseMenu();
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
