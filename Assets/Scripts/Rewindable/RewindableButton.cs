using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableButton : Rewindable
{
    [SerializeField] private Button button = null;
    [System.Serializable]
    struct TimeStamped
    {
        public float timeStamp;
        public bool clicked;

        public TimeStamped(float timeStamp, bool clicked)
        {
            this.timeStamp = timeStamp;
            this.clicked = clicked;
        }
    }
    [SerializeField]
    private List<TimeStamped> history = new List<TimeStamped>();

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
        button.clicked = history[0].clicked;
        if (button.GetComponent<DigicodeButton>())
            button.GetComponent<DigicodeButton>().CheckedButton = history[0].clicked;
    }

    public override void Record(float timeStamp)
    {
        if (history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (history[0].clicked != button.clicked)
            {
                history.Insert(0, new TimeStamped(
                timeStamp,
                button.clicked
                ));
            }
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStamped(
            timeStamp,
            button.clicked
            ));
        }

        // Reset la liste des timestamp si le temps est inférieur au premier timestamp enregistré (après un rewind très long)
        if (history.Count == 1)
        {
            if (timeStamp < history[0].timeStamp)
                history.Clear();
        }

    }

    private void RewindHistory(float timeStamp)
    {
        if (history.Count > 1)
        {
            if (history[0].timeStamp >= timeStamp)
            history.RemoveAt(0);
        }
    }
    public override void StartRewind(float timestamp)
    {
        base.StartRewind(timestamp);
        button.isRewinding = true;
    }


    public override void EndRewind()
    {
        base.EndRewind();
        button.isRewinding = false;
    }

}
