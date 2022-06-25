using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIScreensManager : MonoBehaviour
{
    [SerializeField] private GameObject[] screens;
    // Start is called before the first frame update
    private GameObject currentScreen = null;

    public void OpenScreen(GameObject screen)
    {
        if(screen == currentScreen)
        {
            currentScreen.GetComponent<Animator>().Play("border_Disappear");
            currentScreen = null;
        } else
        {
            if(currentScreen) currentScreen.GetComponent<Animator>().Play("border_Disappear");

            screen.SetActive(true);
            currentScreen = screen;
        }
    }
}
