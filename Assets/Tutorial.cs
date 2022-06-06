using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
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
    [SerializeField] private DragObjects[] docsEcritsToScan;
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
        else if (player.IDCurrentAxis == 0 && dialogueIndex == 10)
        {
            dialogueIndex++;
            if (dialogueIndex < dialogues.Length)
            {
                StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            }
        }
        else if (player.IDCurrentAxis == 0 && dialogueIndex == 14)
        {
            player.QFalse();
            player.DFalse();
        }
        else if (dialogueIndex == 16)
        {
            if (docsEcritsToScan.All(doc => doc.hasBeenTutoScaned == true))
            {
                dialogueIndex++;
                if (dialogueIndex < dialogues.Length)
                {
                    StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
                }
            }
        }
        else if (player.IsInTI && dialogueIndex == 17)
        {
            dialogueIndex++;
            if (dialogueIndex < dialogues.Length)
            {
                StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            }
        }

        else if (player.IDCurrentAxis == 2 && dialogueIndex == 32)
        {
            dialogueIndex++;
            if (dialogueIndex < dialogues.Length)
            {
                StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
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

    public void StartDialogue23()
    {
        if (dialogueIndex == 22)
        {
            dialogueIndex++;
            if (dialogueIndex < dialogues.Length)
            {
                StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            }
        }
    }

    public void StartDialogue25()
    {
        if (dialogueIndex == 24)
        {
            dialogueIndex++;
            if (dialogueIndex < dialogues.Length)
            {
                StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            }
        }
    }

    public void HighlightHelena(GameObject WrittenScreen)
    {
        Image[] panelImages = WrittenScreen.GetComponentsInChildren<Image>();
        foreach(Image image in panelImages)
        {
            if (image.sprite.name == "writtenJournal1")
            {
                image.GetComponent<Highlight>().BeginHighlight();
                break;
            }
        }

        SheetImageScript[] sheetImages = GameObject.Find("TI").GetComponentsInChildren<SheetImageScript>();
        foreach (SheetImageScript sheetImage in sheetImages)
        {
            if (sheetImage.name == "Image (3)")
            {
                sheetImage.GetComponent<Highlight>().BeginHighlight();
            }
        }

    }
    public void StopHighlightHelena()
    {
        PanelTag[] panelImages = GameObject.Find("TI").GetComponentsInChildren<PanelTag>();
        foreach (PanelTag image in panelImages)
        {
            if (image.GetComponent<Image>().sprite.name == "writtenJournal1")
            {
                image.GetComponent<Highlight>().StopHighlight();
                break;
            }
        }

        SheetImageScript[] sheetImages = GameObject.Find("TI").GetComponentsInChildren<SheetImageScript>();
        foreach (SheetImageScript sheetImage in sheetImages)
        {
            if (sheetImage.name == "Image (3)")
            {
                sheetImage.GetComponent<Highlight>().StopHighlight();
            }
        }
    }

    public void HighlightTimelinePointDialogue26()
    {
        TIEntryScript[] panelImages = GameObject.Find("TI").GetComponentsInChildren<TIEntryScript>();
        foreach (TIEntryScript image in panelImages)
        {
            if(image.GetComponent<Image>().sprite != null)
            {
                if (image.GetComponent<Image>().sprite.name == "writtenJournal1")
                {
                    image.GetComponentInParent<TIEntryScript>().gameObject.GetComponent<Highlight>().BeginHighlightChildren();
                    break;
                }
            }
        }
    }
    public void StopHighlightTimelinePointDialogue26()
    {
        TIEntryScript[] panelImages = GameObject.Find("TI").GetComponentsInChildren<TIEntryScript>();
        foreach (TIEntryScript image in panelImages)
        {
            if (image.GetComponent<Image>().sprite != null)
            {
                if (image.GetComponent<Image>().name == "writtenJournal1")
                {
                    image.gameObject.GetComponent<Highlight>().StopHighlightChildren();
                    break;
                }
            }
        }
    }

}
