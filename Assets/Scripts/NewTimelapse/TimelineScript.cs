using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TimelineScript : MonoBehaviour
{
    [System.Serializable]
    public struct SlotPanelImage
    {
        public string ID;
        public bool IsGlitched;
        public bool isTrue;
    }
    [System.Serializable]
    public struct TimelineCheck
    {
        public string[] Date;
        public SlotPanelImage[] PanelImage;
        public bool isTrue;
    }

    public TimelineCheck[] EndAData;
    public TimelineCheck[] EndBData;

    public IADialogue[] DialogueCompletion;


    public void CheckEntry()
    {
        if(GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto && GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 27)
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

        else if (!GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto)
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
                                    if(EndAData[i].PanelImage[y].ID == slot.ID)
                                    {
                                        print("slot" + slot.ID);
                                        print("Data" + EndAData[i].PanelImage[y].ID);
                                        if (slot.isGlitched == EndAData[i].PanelImage[y].IsGlitched)
                                            EndAData[i].PanelImage[y].isTrue = true;
                                    }
                                }
                            }
                        }
                        if (EndAData[i].PanelImage.All(n => n.isTrue == true))
                        {
                            print("hey");
                            EndAData[i].isTrue = true;
                            for (int y = 0; y <= EndAData[i].PanelImage.Length - 1; y++)
                                EndAData[i].PanelImage[y].isTrue = false;
                        }
                    }
                }
            }

            print(EndAData.Count(n => n.isTrue == true) +3);
            if (EndAData.Count(n => n.isTrue == true) + 3 == 3)
                GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().LaunchDialogue(DialogueCompletion[0]);
            else
                GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().LaunchDialogue(DialogueCompletion[EndAData.Count(n => n.isTrue == true)+3]);
            for (int i = 3; i <= 14; i++)
            {
                EndAData[i].isTrue = false;
            }
        }
    }
        IEnumerator ContinueTutorial()
        {
            yield return new WaitForSeconds(15);
            GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
            StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(0));
        }
}
