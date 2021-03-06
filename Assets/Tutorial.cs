using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    public bool activateTuto = true;
    public int tutoEnd = 40;
    [SerializeField] private string[] dialogueNames;
    [SerializeField] private IADialogue[] dialogues;
    [SerializeField] private float[] delays;
    [SerializeField] public TutorialAction[] tutorialActions;
    [SerializeField] private IADialogue[] axisDialogues;
    [SerializeField] private IADialogue TIDialogue;

    [SerializeField] private IAVoiceManager voiceManager = null;
    private PlayerAxisScript player;
    [SerializeField] private DragObjects[] docsEcritsToScan;
    public int dialogueIndex = 0;
    public bool Dialogue32Fini = false;
    public bool Dialogue33Fini = false;
    public bool Dialogue34Fini = false;
    private bool hasSeenVaultOnce = false;
    public bool IsElevatorFinished = false;

    public bool IsDelaying = false;
    private OptionData optionData;

    private bool gameStarted = false;

    private bool axisTutoUnlocked = false;
    private bool axisDialogueOngoing = false;

    public void UnlockTuto()
    {
        try
        {
            optionData = GameObject.Find("OptionsData").GetComponent<OptionData>();
        }
        catch
        {
            optionData = null;
        }
        if (optionData != null)
        {
            if (optionData.TutoActivated)
            {
                activateTuto = true;
            }
            else
            {
                activateTuto = false;
            }
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BeginGame()
    {
        player = GameObject.Find("Player").GetComponent<PlayerAxisScript>();
        gameStarted = true;
        if (activateTuto)
        {
            StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            voiceManager.OnDialogueFinished += DialogueFinished;
            for (int i = 0; i < tutorialActions.Length; i++)
            {
                if (tutorialActions[i])
                {
                    tutorialActions[i].OnTutoStart();
                }
            }
        }
        StartCoroutine(ElevatorCooldown());
    }

    private void Update()
    {
        if(activateTuto && gameStarted)
        {
            if(Input.GetButtonDown("AxisTutorial") && axisTutoUnlocked)
            {
                if(player.IsInTI)
                {
                    if (!axisDialogueOngoing)
                    {
                        voiceManager.LaunchDialogue(TIDialogue);
                        axisDialogueOngoing = true;
                    }
                } else
                {
                    LaunchAxisDialogue(player.IDCurrentAxis);
                }
            }
            if (dialogueIndex == 0 && IsElevatorFinished)
            {
                if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                    DialogueFinished();
                dialogueIndex++;
                if (dialogueIndex < tutoEnd)
                {
                    StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
                }
            }
            //if (player.IDCurrentAxis == 4 && dialogueIndex >= 30 && !hasSeenVaultOnce && !GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
            //{
            //    GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().LaunchDialogue(GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().RandomAntiCasierFirstDialogue);
            //    hasSeenVaultOnce = true;
            //}

            // La plupart des triggers pour continuer le tuto
            if (player.IDCurrentAxis == 0 && dialogueIndex == 1)
            {
                if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                    DialogueFinished();
                dialogueIndex++;
                if (dialogueIndex < tutoEnd)
                {
                    StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
                }
            }
            //else if (player.IDCurrentAxis == 5 && dialogueIndex == 3)
            //{
            //    if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
            //        DialogueFinished();
            //    dialogueIndex++;
            //    if (dialogueIndex < dialogues.Length)
            //    {
            //        StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            //    }
            //}
            //else if (player.IDCurrentAxis == 1 && dialogueIndex == 7)
            //{
            //    if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
            //        DialogueFinished();
            //    dialogueIndex++;
            //    if (dialogueIndex < dialogues.Length)
            //    {
            //        StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            //    }
            //}
            //else if (player.IDCurrentAxis == 0 && dialogueIndex == 10)
            //{
            //    if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
            //        DialogueFinished();
            //    dialogueIndex++;
            //    if (dialogueIndex < dialogues.Length)
            //    {
            //        StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            //    }
            //}
            //else if (player.IDCurrentAxis == 0 && dialogueIndex == 14)
            //{
            //    player.QFalse();
            //    player.DFalse();
            //}
            //else if (dialogueIndex == 16)
            //{
            //    if (docsEcritsToScan.All(doc => doc.hasBeenTutoScaned == true))
            //    {
            //        if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
            //            DialogueFinished();
            //        dialogueIndex++;
            //        if (dialogueIndex < dialogues.Length)
            //        {
            //            StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            //        }
            //    }
            //}
            //else if (player.IsInTI && dialogueIndex == 17)
            //{
            //    if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
            //        DialogueFinished();
            //    dialogueIndex++;
            //    if (dialogueIndex < dialogues.Length)
            //    {
            //        StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            //    }
            //}

            //else if (player.IDCurrentAxis == 2 && dialogueIndex == 32 && !GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying && Dialogue32Fini)
            //{
            //    DialogueFinished();
            //    dialogueIndex++;
            //    if (dialogueIndex < dialogues.Length)
            //    {
            //        StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            //    }
            //}
            //else if (player.IDCurrentAxis == 3 && dialogueIndex == 34 && !GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying && Dialogue34Fini)
            //{
            //    DialogueFinished();
            //    dialogueIndex++;
            //    if (dialogueIndex < dialogues.Length)
            //    {
            //        StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            //    }
            //}


            //Fait en sorte que l'IA puisse r?p?ter sa derni?re phrase
            if (Input.GetKeyDown(KeyCode.H) && !voiceManager.DialogueHappening && !IsDelaying)
            {
                voiceManager.LaunchDialogue(dialogues[dialogueIndex]);
                voiceManager.IsRepeating = true;
            }
        }
        

    }

    public void DialogueFinished()
    {
        if (tutorialActions[dialogueIndex])
        {
            tutorialActions[dialogueIndex].ExecuteAction();
        }
        if (dialogues[dialogueIndex].IsFolowedByAnotherDialogue)
        {
            dialogueIndex++;
            if (dialogueIndex < tutoEnd)
            {
                StartCoroutine(LaunchNextDialogue(delays[dialogueIndex]));
            } 
        }
        if (!axisTutoUnlocked && dialogueIndex == tutoEnd)
        {
            axisTutoUnlocked = true;
            voiceManager.OnDialogueFinished -= DialogueFinished;
            voiceManager.OnDialogueFinished += AxisDialogueFinished;
        }

    }

    public void AxisDialogueFinished()
    {
        axisDialogueOngoing = false;
    }

    public IEnumerator LaunchNextDialogue(float delay)
    {
        IsDelaying = true;
        yield return new WaitForSeconds(delay);
        while(voiceManager.DialogueHappening)
        {
            yield return new WaitForSeconds(0.5f);
        }
        IsDelaying = false;
        voiceManager.LaunchDialogue(dialogues[dialogueIndex]);
        if(tutorialActions[dialogueIndex])
        {
            tutorialActions[dialogueIndex].OnDialogueStart();
        }
    }

    public void LaunchAxisDialogue(int axis)
    {
        if(!axisDialogueOngoing)
        {
            voiceManager.LaunchDialogue(axisDialogues[axis]);
            axisDialogueOngoing = true;
        }
    }

    public void StartDialogue23()
    {
        if (dialogueIndex == 22)
        {
            if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                DialogueFinished();
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
            if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                DialogueFinished();
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
            if (image.sprite.name == "Journal de bord 1")
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
            if (image.GetComponent<Image>().sprite.name == "Journal de bord 1")
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
                if (image.GetComponent<Image>().sprite.name == "Journal de bord 1")
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
                if (image.GetComponent<Image>().name == "Journal de bord 1")
                {
                    image.gameObject.GetComponent<Highlight>().StopHighlightChildren();
                    break;
                }
            }
        }
    }
    public void FinishDialogue32()
    {
        Dialogue32Fini = true;
    }
    public void FinishDialogue33()
    {
        Dialogue33Fini = true;
    }
    public void FinishDialogue34()
    {
        Dialogue34Fini = true;
    }

    public void EndTuto()
    {
        activateTuto = false;
        GameObject.Find("Player").GetComponent<PlayerAxisScript>().isInTuto = false;
    }
    IEnumerator ElevatorCooldown()
    {
        yield return new WaitForSeconds(15);
        IsElevatorFinished = true;
    }
}
