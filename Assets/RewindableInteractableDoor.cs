using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableInteractableDoor : Rewindable
{
    private InteractableDoor _interactableDoor = null;
    [System.Serializable]
    struct TimeStamped
    {
        public float timeStamp;
        public bool IsOpen;
        public TimeStamped(float timeStamp, bool IsOpen)
        {
            this.timeStamp = timeStamp;
            this.IsOpen = IsOpen;
        }
    }
    [SerializeField]
    private List<TimeStamped> history = new List<TimeStamped>();

    public override void Start()
    {
        base.Start();
        _interactableDoor = this.GetComponent<InteractableDoor>();
    }

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
        _interactableDoor.IsOpen = history[0].IsOpen;
    }

    public override void Record(float timeStamp)
    {
        if (history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (history[0].IsOpen != _interactableDoor.IsOpen)
                history.Insert(0, new TimeStamped(
                timeStamp,
                _interactableDoor.IsOpen
                ));
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStamped(
            timeStamp,
            _interactableDoor.IsOpen
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
}
