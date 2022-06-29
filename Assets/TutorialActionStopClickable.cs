using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActionStopClickable : TutorialAction
{
    [SerializeField] private GameObject[] clickables;
    public override void OnTutoStart()
    {
        base.OnTutoStart();
        List<IClickable> clickable;
        foreach(GameObject obj in clickables)
        {
            clickable = new List<IClickable>();
            clickable.AddRange(obj.GetComponents<IClickable>());
            clickable.AddRange(obj.GetComponentsInChildren<IClickable>());
            foreach(IClickable click in clickable)
            {
                click.SetClickable(false);
                click.GetOnClicked += PlayIAMessage;
            }
            
        }
    }

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        List<IClickable> clickable;
        foreach (GameObject obj in clickables)
        {
            clickable = new List<IClickable>();
            clickable.AddRange(obj.GetComponents<IClickable>());
            clickable.AddRange(obj.GetComponentsInChildren<IClickable>());
            foreach (IClickable click in clickable)
            {
                click.SetClickable(true);
                click.GetOnClicked -= PlayIAMessage;
            }
        }
    }

    private void PlayIAMessage()
    {
        FindObjectOfType<IAVoiceManager>().LaunchRandomFocusDialogue();
    }
}
