using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
[HelpURL("https://docs.google.com/document/d/1ybLDsyvFSfyHyQXm3qOMYwDXoBWAWYOS2rm46UGMBqg/edit?usp=sharing")]

public class ReparationCalibrageScript : MonoBehaviour
{
    [HideInInspector]
    public TextMeshPro screenText = null;
    private string currentCode = "";
    
    public string CurrentCode
    {
        get
        {
            return currentCode;
        }
        set
        {
            if (currentCode != value)
                currentCode = value;
            screenText.text = currentCode;
            if (currentCode.Length == requiredCode.Length - 1)
            {
                if (repaired == true)
                {
                    onCodeRewinded?.Invoke();

                    repaired = false;
                }
            }

        }
    }



    [Header("Inscrire ici le code à inscrire pour valider cette réparation")]
    [Header("Si vous avez du mal à paramétrer ce script, cliquez sur le point d'interrogation")]

    public string requiredCode = "1234";
    [Header("Préciser ici ce qui doit se passer lorsque le joueur a inscrit le bon code")]
    public UnityEvent onCodeEntered;
    [Header("Préciser ici ce qui doit se passer lorsque la réparation est annulée par le rewind")]
    public UnityEvent onCodeRewinded;

    [Header("Placer ici les deux prefab EraseButton (ceux marqués d'une croix)")]
    [SerializeField] private ReparationCalibrageButton eraseEntireCodeButton = null;
    [SerializeField] private ReparationCalibrageButton eraseCodeButton = null;

    [HideInInspector]
    public bool repaired = false;

    private TimeManager _timeManager;

    public AudioSource errorSound;


    // Start is called before the first frame update
    void Start()
    {
        _timeManager = FindObjectOfType<TimeManager>().GetComponent<TimeManager>();
        screenText = gameObject.GetComponentInChildren<TextMeshPro>();
    }

    public void CheckCode()
    {
        if (currentCode == requiredCode)
        {
            if (repaired == false)
            {
                onCodeEntered?.Invoke();
                if (!_timeManager.rewindManager.isRewinding && !_timeManager.IsTimeStopped)
                {
                    GetComponent<AudioSource>().Play();
                    GetComponent<AudioSource>().time = 0;
                }
                repaired = true;
            }
        }
        else
        {
            if (repaired == true)
            {
                repaired = false;
                onCodeRewinded?.Invoke();
            }
            else
            {
                if (!_timeManager.rewindManager.isRewinding && !_timeManager.IsTimeStopped)
                {
                    errorSound.Play();
                    errorSound.time = 0;
                }
            }
        }

    }

    //Si le joueur a appuyé sur le bouton effacer et utilise le rewind, le code précédément effacé réapparait
    public void UnclearEntireCode()
    {
        currentCode = eraseEntireCodeButton.codeMemory[0];
        eraseEntireCodeButton.codeMemory.RemoveAt(0);
    }

    public void UnclearCode()
    {
        currentCode = eraseCodeButton.codeMemory[0];
        eraseCodeButton.codeMemory.RemoveAt(0);
    }

}
