using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public bool lightEnabled = false;
    public float timerLightEnabled;

    // Le Level Designer peut changer cette variable afin de fixer une durée pour la lumière allumée
    [SerializeField]
    public float timerLightEnabledMax;

    public float timerLightDisabled;

    // Le Level Designer peut changer cette variable afin de fixer une durée pour la lumière éteinte
    [SerializeField]
    public float timerLightDisabledMax;

    public float multiplier = 1f;

    private GameObject TimeManager;

    [SerializeField] private bool _isTimeInteractable = true;

    // Start is called before the first frame update
    void Start()
    {
        TimeManager = GameObject.Find("TimeManager");
    }

    // Update is called once per frame
    void Update()
    {
        // Branche le multiplier au TimeManager
        if (_isTimeInteractable)
            multiplier = TimeManager.GetComponent<TimeManager>().multiplier;

        //Gère la lumière allumée
        if (timerLightEnabled >= 0 && timerLightEnabled <= timerLightEnabledMax)
        {
            if (_isTimeInteractable)
                timerLightEnabled -= Time.deltaTime * multiplier;
            else
                timerLightEnabled -= Time.unscaledDeltaTime;
        }
        else
        {
            lightEnabled = false;
        }

        //Gère la lumière éteinte
        if (timerLightDisabled >= 0 && timerLightDisabled <= timerLightDisabledMax)
        {
            if (_isTimeInteractable)
                timerLightDisabled -= Time.deltaTime * multiplier;
            else
                timerLightDisabled -= Time.unscaledDeltaTime ;
        }
        else
        {
            lightEnabled = true;
        }

        //Active la lumière et reset les timers en fonction de si la lumière doit être allumée ou éteinte
        if (lightEnabled == false)
        {
            GetComponent<Light>().intensity = 0;
            timerLightEnabled = timerLightEnabledMax;
        }
        else 
        {
            GetComponent<Light>().intensity = 1;
            timerLightDisabled = timerLightDisabledMax;
        }
    }
}
