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

    public Dialogue[] RandomAntiCasierDialogue;

    [SerializeField] int i = 0;

    private bool dialogueHappening = false;

    private AudioSource source;

    private TMP_Text txt = null;
    private bool _inCooldown = false;

    public Action OnDialogueFinished;

    private void Awake()
    {

         source = GetComponent<AudioSource>();
        txt = GameObject.Find("IASubtitle").GetComponent<TMP_Text>();
    }

    public void LaunchDialogue(string dialogueName)
    {
        DialogueTexts = Array.Find(DialogueList, Dialogue => Dialogue.DialogueName == dialogueName).DialogueTexts;
        DialogueSounds = Array.Find(DialogueList, Dialogue => Dialogue.DialogueName == dialogueName).DialogueSounds;
        dialogueHappening = true;
        i = 0;
        Play();
    }

    public void LaunchDialogue(IADialogue dialogue)
    {
        DialogueTexts = dialogue.DialogueTexts;
        DialogueSounds = dialogue.DialogueSounds;
        dialogueHappening = true;
        i = 0;
        Play();
    }

    public void LaunchRandomAntiCasierDialogue()
    {
        int iCasier = UnityEngine.Random.Range(0,6);
        DialogueTexts = RandomAntiCasierDialogue[iCasier].DialogueTexts;
        DialogueSounds = RandomAntiCasierDialogue[iCasier].DialogueSounds;
        dialogueHappening = true;
        i = 0;
        Play();
    }
    private void Update()
    {
        //if (Input.GetKey(KeyCode.A))
            //LaunchDialogue("ComplétionHorsTuto11");

        if(dialogueHappening)
        {
            if (!source.isPlaying && !_inCooldown)
                StartCoroutine(Cooldown());
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
            dialogueHappening = false;
            txt.text = "";

            OnDialogueFinished?.Invoke();
        }
        else if(i != 0)
        {
            Play();
        }
        _inCooldown = false;
    }


}
