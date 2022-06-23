using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;


public class IAVoiceManager : MonoBehaviour
{
    public string[] DialogueTexts;
    public AudioClip[] DialogueSounds;

    public Dialogue[] DialogueList;

    public IADialogue[] RandomAntiCasierDialogues;
    public IADialogue RandomAntiCasierFirstDialogue;
    public IADialogue NebuleuseDialogue;
    public IADialogue EndADialogue;
    public IADialogue EndBDialogue;

    [SerializeField] int i = 0;

    public bool DialogueHappening = false;

    private AudioSource source;

    private TMP_Text txt = null;
    private bool _inCooldown = false;

    public Action OnDialogueFinished;

    public bool currentDialogueNotInTuto = false;

    public bool IsRepeating = false;
    private OptionData optionData;

    public bool IsTalkingTutorial = false;


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

        source = GetComponent<AudioSource>();
        txt = GameObject.Find("IASubtitle").GetComponent<TMP_Text>();
    }

    public void LaunchDialogue(string dialogueName)
    {
        DialogueTexts = Array.Find(DialogueList, Dialogue => Dialogue.DialogueName == dialogueName).DialogueTexts;
        DialogueSounds = Array.Find(DialogueList, Dialogue => Dialogue.DialogueName == dialogueName).DialogueSounds;
        DialogueHappening = true;
        i = 0;
        Play();
    }

    public void LaunchDialogue(IADialogue dialogue)
    {
        DialogueTexts = dialogue.DialogueTexts;
        DialogueSounds = dialogue.DialogueSounds;
        DialogueHappening = true;
        currentDialogueNotInTuto = dialogue.notInTutorialDialogue;
        i = 0;
        if (dialogue.ShouldStopPlayerInteraction)
            IsTalkingTutorial = true;
        Play();
    }

    public void LaunchRandomAntiCasierDialogue()
    {
        int iCasier = UnityEngine.Random.Range(0,6);
        DialogueTexts = RandomAntiCasierDialogues[iCasier].DialogueTexts;
        DialogueSounds = RandomAntiCasierDialogues[iCasier].DialogueSounds;
        DialogueHappening = true;
        i = 0;
        Play();
    }
    private void Update()
    {
        //if (Input.GetKey(KeyCode.A))
            //LaunchDialogue("ComplétionHorsTuto11");

        if(DialogueHappening)
        {
            if (!source.isPlaying && !_inCooldown)
                StartCoroutine(Cooldown());
        }
        if (optionData)
        {
            if (!optionData.IAActivated)
                GetComponent<AudioSource>().volume = 0;
            else
                GetComponent<AudioSource>().volume = 1;

        }
    }

    public void Play()
    {
        source.clip = DialogueSounds[i];
        source.Play();
        txt.text = DialogueTexts[i];
    }
    IEnumerator Cooldown()
    {
        _inCooldown = true;
        i += 1;
        yield return new WaitForSeconds(1);
        if (i >= DialogueTexts.Length && i != 0)
        {
            DialogueTexts = null;
            DialogueSounds = null;
            DialogueHappening = false;
            IsTalkingTutorial = false;
            txt.text = "";

            Debug.Log("cooldown");
            if(!currentDialogueNotInTuto && !IsRepeating)
            {
                OnDialogueFinished?.Invoke();
            }
            IsRepeating = false;

        }
        else if(i != 0)
        {
            Play();
        }
        _inCooldown = false;
    }


}
