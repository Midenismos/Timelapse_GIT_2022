using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private bool activateTuto = true;
    [SerializeField] private string[] dialogueNames;
    [SerializeField] private float[] delays;

    [SerializeField] private IAVoiceManager voiceManager = null;

    private int dialogueIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(activateTuto)
        {
            StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            voiceManager.OnDialogueFinished += DialogueFinished;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DialogueFinished()
    {
        dialogueIndex++;
        if(dialogueIndex < dialogueNames.Length)
        {
            StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
        }
    }

    private IEnumerator LaunchNextDialogue(float delay)
    {
        yield return new WaitForSeconds(delay);
        voiceManager.LaunchDialogue(dialogueNames[dialogueIndex]);

    }
}
