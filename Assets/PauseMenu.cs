using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool GamePaused = false;


    public GameObject pauseMenu;
    public GameObject[] Menus;

    public bool isPauseAllowed = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            } 
            else if (isPauseAllowed)
            {
                Pause();
            }
        }
    }

    public void Resume ()
    {
        pauseMenu.SetActive(false);
        foreach (GameObject menu in Menus)
            menu.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void Pause ()
    {
        pauseMenu.SetActive(true);
        Menus[0].SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

}
