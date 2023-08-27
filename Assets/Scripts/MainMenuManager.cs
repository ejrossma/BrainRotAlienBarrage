using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void AdjustVolume(float sliderVal)
    {
        //AudioManager.SetFloat("Volume", Mathf.log10(sliderVal) * 20);
        PlayerPrefs.SetFloat("Volume", sliderVal);
    }
}
