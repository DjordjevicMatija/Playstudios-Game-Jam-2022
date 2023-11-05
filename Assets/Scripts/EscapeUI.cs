using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeUI : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == true)
            {
                Resume();
            }
            else
            { 
                Pause();
            }
        }
    }

    public void Resume()
    { 
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Log");
    }

    void Pause()
    { 
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void MainMenu() {
        Debug.Log("Menu");
        SceneManager.LoadScene("MainMenu");
    }
}
