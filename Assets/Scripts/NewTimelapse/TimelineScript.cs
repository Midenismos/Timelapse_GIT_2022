using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TimelineScript : MonoBehaviour
{
    [System.Serializable]
    public struct TimelineCheck
    {
        public string Date;
        public string ID;
        public bool IsGlitched;
        public bool isTrue;
    }

    public TimelineCheck[] EndAData;
    public TimelineCheck[] EndBData;

    public IADialogue[] DialogueCompletion;


    public void CheckEntry()
    {
        if(GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto)
        {
            foreach(TIEntryScript Entry in GameObject.Find("TI").GetComponentsInChildren<TIEntryScript>())
            {
                for (int i = 0; i <= 2; i++ )
                {
                    print(i);
                    if (Entry.Date == EndAData[i].Date)
                    {

                        foreach ( SheetImageScript slot in Entry.Slots)
                        {

                            if (slot.ID == EndAData[i].ID)
                            {
                                if (slot.isGlitched == EndAData[i].IsGlitched)
                                {
                                    print(Entry.Date);
                                    print(slot.ID);
                                    EndAData[i].isTrue = true;
                                }
                            }
                        }
                    }
                }
            }
            print(EndAData.Count(n => n.isTrue == true));
            for (int i = 0; i <= 2; i++)
            {
                EndAData[i].isTrue = false;
            }
        }

    }
}
