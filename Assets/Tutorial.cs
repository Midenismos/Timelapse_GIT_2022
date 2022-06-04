using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Tutorial : MonoBehaviour
{
    [SerializeField] private bool activateTuto = true;
    [SerializeField] private string[] dialogueNames;
    [SerializeField] private IADialogue[] dialogues;
    [SerializeField] private float[] delays;
    [SerializeField] private TutorialAction[] tutorialActions;

    [SerializeField] private IAVoiceManager voiceManager = null;
    private PlayerAxisScript player;

    [SerializeField] private CamButton[] SliderButtons;
    public int dialogueIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerAxisScript>();
        if (activateTuto)
        {
            StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            voiceManager.OnDialogueFinished += DialogueFinished;

            for (int i = 0; i < tutorialActions.Length; i++)
            {
                if(tutorialActions[i])
                {
                    tutorialActions[i].OnTutoStart();
                }
            }
        }
    }

    private void Update()
    {
        // La plupart des triggers pour continuer le tuto
        if (player.IDCurrentAxis == 0 && dialogueIndex == 1)
        {
            dialogueIndex++;
            if (dialogueIndex < dialogues.Length)
            {
                StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            }
        }
        else if (player.IDCurrentAxis == 5 && dialogueIndex == 3)
        {
            dialogueIndex++;
            if (dialogueIndex < dialogues.Length)
            {
                StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            }
        }
        else if (dialogueIndex == 5)
        {
            if(SliderButtons.All(button => button.isSeenInTuto == true))
            {
                dialogueIndex++;
                if (dialogueIndex < dialogues.Length)
                {
                    StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
                }
            }
        }
        else if (player.IDCurrentAxis == 1 && dialogueIndex == 7)
        {
            foreach (CamButton button in SliderButtons)
                button.isSeenInTuto = false;
            dialogueIndex++;
            if (dialogueIndex < dialogues.Length)
            {

                StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            }
        }
        else if (dialogueIndex == 8)
        {
            if (SliderButtons.All(button => button.isSeenInTuto == true))
            {
                dialogueIndex++;
                if (dialogueIndex < dialogues.Length)
                {
                    StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
                }
            }
        }

    }

    private void DialogueFinished()
    {
        if (tutorialActions[dialogueIndex])
        {
            tutorialActions[dialogueIndex].ExecuteAction();
            print(dialogueIndex);
        };
        if(dialogues[dialogueIndex].IsFolowedByAnotherDialogue)
        {
            print("hey");
            dialogueIndex++;
            if (dialogueIndex < dialogues.Length)
            {
                StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            }
        }

    }

    public IEnumerator LaunchNextDialogue(float delay)
    {
        yield return new WaitForSeconds(delay);
        voiceManager.LaunchDialogue(dialogues[dialogueIndex]);
        if(tutorialActions[dialogueIndex])
            tutorialActions[dialogueIndex].OnDialogueStart();
    }
}
