using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialActionUnityEvent : TutorialAction
{
    public UnityEvent actionsAtStart;
    public UnityEvent actions;
    public UnityEvent actionsAtDialogueStart;
    public override void OnTutoStart()
    {
        actionsAtStart?.Invoke();
    }

    public override void ExecuteAction()
    {
        actions?.Invoke();
    }

    public override void OnDialogueStart()
    {
        actionsAtDialogueStart?.Invoke();
    }
}
