using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Video;
public class TimelineScript : MonoBehaviour
{
    [System.Serializable]
    public struct SlotPanelImage
    {
        public string ID;
        public bool IsGlitched;
        public bool isTrue;
        public bool GlitchChecked;
    }
    [System.Serializable]
    public struct TimelineCheck
    {
        public string[] Date;
        public SlotPanelImage[] PanelImage;
        public bool isTrue;
    }

    public TimelineCheck[] EndAData;

    public IADialogue[] DialogueCompletion;

    private int corruptedNumber = 0;
    [SerializeField] private int corruptedNumberRequired = 9;
    [SerializeField] private GameObject reportButton = null;
    [SerializeField] private VideoClip[] EndingVideos = null;
    public void CheckEntry()
    {
        if(GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto && GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 27 && !GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
        {
            foreach(TIEntryScript Entry in GameObject.Find("TI").GetComponentsInChildren<TIEntryScript>())
            {
                for (int i = 0; i <= 2; i++ )
                {
                    if (Entry.Date == EndAData[i].Date[0])
                    {
                        foreach (SheetImageScript slot in Entry.Slots)
                        {
                            for (int y = 0; y <= EndAData[i].PanelImage.Length - 1; y++)
                            {
                                if(EndAData[i].PanelImage[y].ID == slot.ID)
                                {
                                    if (slot.isGlitched == EndAData[i].PanelImage[y].IsGlitched)
                                        EndAData[i].isTrue = true;
                                }
                            }
                        }
                    }
                }
            }
            GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().LaunchDialogue(DialogueCompletion[EndAData.Count(n => n.isTrue == true)]);
            if (EndAData.Count(n => n.isTrue == true) == 3)
                StartCoroutine(ContinueTutorial());
            for (int i = 0; i <= 2; i++)
            {
                EndAData[i].isTrue = false;
            }
        }

        else if (!GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto || GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex >= 30 && !GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
        {
            foreach (TIEntryScript Entry in GameObject.Find("TI").GetComponentsInChildren<TIEntryScript>())
            {
                if(!Entry.IsTuto && Entry.GetComponent<DragObjects>().IsFixedInTI)
                {
                    for (int i = 3; i <= 14; i++)
                    {
                        
                        if (int.Parse(Entry.Date) >= int.Parse(EndAData[i].Date[0]) && int.Parse(Entry.Date) <= int.Parse(EndAData[i].Date[1]))
                        {
                            foreach (SheetImageScript slot in Entry.Slots)
                            {
                                for (int y = 0; y <= EndAData[i].PanelImage.Length - 1; y++)
                                {

                                    if (EndAData[i].PanelImage[y].ID == slot.ID)
                                    {
                                        if (EndAData[i].PanelImage[y].IsGlitched == true && slot.isGlitched == EndAData[i].PanelImage[y].IsGlitched && EndAData[i].PanelImage[y].GlitchChecked == false)
                                        {
                                            corruptedNumber += 1;
                                            EndAData[i].PanelImage[y].GlitchChecked = true;
                                        }
                                        if (EndAData[i].PanelImage[y].IsGlitched == false && slot.isGlitched == false)
                                            EndAData[i].PanelImage[y].isTrue = true;
                                        else if (EndAData[i].PanelImage[y].IsGlitched == true)
                                            EndAData[i].PanelImage[y].isTrue = true;


                                    }
                                }
                            }
                        }
                        if (EndAData[i].PanelImage.All(n => n.isTrue == true))
                        {
                            EndAData[i].isTrue = true;
                            for (int y = 0; y <= EndAData[i].PanelImage.Length - 1; y++)
                                EndAData[i].PanelImage[y].isTrue = false;
                        }
                    }
                }
            }
            print(corruptedNumber);

            print(EndAData.Count(n => n.isTrue == true) +3);
            if (EndAData.Count(n => n.isTrue == true) + 3 == 3)
                GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().LaunchDialogue(DialogueCompletion[0]);
            else
                GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().LaunchDialogue(DialogueCompletion[EndAData.Count(n => n.isTrue == true)+3]);
            if(EndAData.Count(n => n.isTrue == true) + 3 == 15)
                StartCoroutine(SpawnRepportButton());



            for (int i = 3; i <= 14; i++)
            {
                EndAData[i].isTrue = false;
                for (int y = 0; y <= EndAData[i].PanelImage.Length - 1; y++)
                    EndAData[i].PanelImage[y].GlitchChecked = false;

            }
            corruptedNumber = 0;
        }
    }
    IEnumerator ContinueTutorial()
    {
        //if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
        //    GameObject.Find("TutorialManager").GetComponent<Tutorial>().DialogueFinished();
        ////GameObject.Find("Player").GetComponent<PlayerAxisScript>().ZFalse();
        yield return new WaitForSeconds(15);
        //GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
        //StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(0));
    }


    public void StartEnding()
    {
        VideoPlayer EndingPlayer = GameObject.Find("VideoEnding").GetComponent<VideoPlayer>();
        foreach (TIEntryScript Entry in GameObject.Find("TI").GetComponentsInChildren<TIEntryScript>())
        {
            if (!Entry.IsTuto && Entry.GetComponent<DragObjects>().IsFixedInTI)
            {
                for (int i = 0; i <= EndAData.Length-1; i++)
                {

                    if (int.Parse(Entry.Date) >= int.Parse(EndAData[i].Date[0]) && int.Parse(Entry.Date) <= int.Parse(EndAData[i].Date[1]))
                    {
                        foreach (SheetImageScript slot in Entry.Slots)
                        {
                            for (int y = 0; y <= EndAData[i].PanelImage.Length - 1; y++)
                            {

                                if (EndAData[i].PanelImage[y].ID == slot.ID)
                                {
                                    if (EndAData[i].PanelImage[y].IsGlitched == true && slot.isGlitched == EndAData[i].PanelImage[y].IsGlitched && EndAData[i].PanelImage[y].GlitchChecked == false)
                                    {
                                        corruptedNumber += 1;
                                        EndAData[i].PanelImage[y].GlitchChecked = true;
                                    }
                                    if (EndAData[i].PanelImage[y].IsGlitched == false && slot.isGlitched == false)
                                        EndAData[i].PanelImage[y].isTrue = true;
                                    else if (EndAData[i].PanelImage[y].IsGlitched == true)
                                        EndAData[i].PanelImage[y].isTrue = true;


                                }
                            }
                        }
                    }
                    if (EndAData[i].PanelImage.All(n => n.isTrue == true))
                    {
                        EndAData[i].isTrue = true;
                        for (int y = 0; y <= EndAData[i].PanelImage.Length - 1; y++)
                            EndAData[i].PanelImage[y].isTrue = false;
                    }
                }
            }
        }
        if (EndAData.Count(n => n.isTrue == true) + 3 == 15)
        {
            if (corruptedNumber == 9)
            {
                EndingPlayer.clip = EndingVideos[1];
                EndingPlayer.Play();
                GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().LaunchDialogue(GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().EndBDialogue);
            }
            //image B
            else
            {
                EndingPlayer.clip = EndingVideos[2];
                EndingPlayer.Play();
                GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().LaunchDialogue(GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().EndADialogue);

            }
            //image A
        }
        else
        {
            //Fin ratée
            EndingPlayer.clip = EndingVideos[0];
            EndingPlayer.Play();
        }
    }
}
