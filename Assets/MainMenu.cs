using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void ButtonQuitter() {
        Application.Quit();
        Debug.Log("Game closed");
    }

    public void ButtonStart() {
        SceneManager.LoadScene("NewTimelapsePrototype");
    }

    public AudioSource audiosource;
    public Slider slider;
    public Text TextVolume;

    public void SliderChange()

    {
        audiosource.volume = slider.value;
        TextVolume.text = "Volume " + (audiosource.volume * 100).ToString("00") + "%";
    }
 
}
