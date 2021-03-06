using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public void ButtonQuitter() {
        Application.Quit();
        //Debug.Log("Pourquoi vouloir quitter ?");
    }

    public void ButtonStart() {
        //SceneManager.LoadScene("NewTimelapsePrototype");
        FindObjectOfType<Tutorial>().UnlockTuto();
        FindObjectOfType<PlayerAxisScript>().UnLockTuto();
        FindObjectOfType<Tutorial>().BeginGame();
        FindObjectOfType<IntroductionSequence>().BeginGame();
        FindObjectOfType<PauseMenu>(true).isPauseAllowed = true;

        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(null);

    }

    public AudioMixer Mixer;
    public Slider slider;
    public Text TextVolume;
    private OptionData optionData;
    [SerializeField] private GameObject buttonDesactiverVoixIA;


    private void Awake()
    {
        try
        {
            optionData = GameObject.Find("OptionsData").GetComponent<OptionData>();
        }
        catch
        {
            optionData = null;
        }
        if(optionData)
        {
            buttonDesactiverVoixIA.GetComponentInChildren<Text>().text = optionData.IAActivated ? "DESACTIVER LA VOIX DE L'IA" : "ACTIVER LA VOIX DE L'IA";
            slider.value = GameObject.Find("OptionsData").GetComponent<OptionData>().VolumeSliderValue;
        }
    }
    public void SliderChange()

    {
        //audiosource.volume = slider.value;
        Mixer.SetFloat("Volume", Mathf.Log10(slider.value) * 20);
        TextVolume.text = "Volume " + (slider.value * 100).ToString("00") + "%";
        if(optionData)
            optionData.VolumeSliderValue = slider.value;
    }

    public void ToggleTuto()
    {
        if (optionData)
        {
            optionData.TutoActivated = !optionData.TutoActivated;
            GameObject.Find("ButtonDesactiverTuto").GetComponentInChildren<Text>().text = optionData.TutoActivated ? "DESACTIVER LE TUTORIEL" : "ACTIVER LE TUTORIEL";
        }
    }

    public void ToggleIAActivated()
    {
        if (optionData)
        {
            optionData.IAActivated = !optionData.IAActivated;
            GameObject.Find("ButtonDesactiverVoixIA").GetComponentInChildren<Text>().text = optionData.IAActivated ? "DESACTIVER LA VOIX DE L'IA" : "ACTIVER LA VOIX DE L'IA";
        }
    }

}
