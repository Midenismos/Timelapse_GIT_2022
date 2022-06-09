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
            print(EndAData.Count(n => n.isTrue == true));
            GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().LaunchDialogue(DialogueCompletion[EndAData.Count(n => n.isTrue == true)]);
            if (EndAData.Count(n => n.isTrue == true) == 3)
                StartCoroutine(ContinueTutorial());
            for (int i = 0; i <= 2; i++)
            {
                EndAData[i].isTrue = false;
            }
        }


        //POUR APRES TUTO
        //if (int.Parse(Entry.Date) >=int.Parse(EndAData[i].Date[0]) && int.Parse(Entry.Date) <= int.Parse(EndAData[i].Date[1]))
        IEnumerator ContinueTutorial()
        {
            yield return new WaitForSeconds(15);
            GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
            StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(0));
        }
    }
}
