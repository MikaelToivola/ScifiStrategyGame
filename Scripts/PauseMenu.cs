using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool paused = false;

    Canvas canvas;
    UImanager uiManager;
    GameObject menu;
    //bool freezeTime = false;
    //private string[] scenePaths;

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        uiManager = canvas.GetComponent<UImanager>();
        menu = canvas.transform.Find("Menu").gameObject;
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) ResumeGame();
            else PauseGame();
        }
        
    }
    public void PauseGame()
    {
        paused = true;
        Time.timeScale = 0;
        //Activate menu
        menu.SetActive(true);
    }

    public void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1;
        menu.SetActive(false);

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
