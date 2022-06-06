using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineScript : MonoBehaviour
{
    [System.Serializable]
    public struct TimelineCheck
    {
        public string Date;
        public string ID;
        public bool IsGlitched;
    }

    public TimelineCheck[] EndAData;
    public TimelineCheck[] EndBData;

    public IADialogue[] DialogueCompletion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckEntry()
    {
        if(GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto)
        {
            foreach(TIEntryScript Entry in GameObject.Find("TI").GetComponentsInChildren<TIEntryScript>())
            {
                for (int i = 0; i <= 2; i++ )
                {
                    if (Entry.Date == EndAData[i].Date)
                    {
                        print("hey");
                        foreach( SheetImageScript slot in Entry.Slots)
                        {
                            print("hey");

                            if (slot.ID == EndAData[i].ID)
                            {
                                print("hey");

                                if (slot.isGlitched == EndAData[i].IsGlitched)
                                {
                                    print("hey");
                                    GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().LaunchDialogue(DialogueCompletion[0]);

                                }

                            }
                        }
                    }
                }
            }
        }

    }
}
