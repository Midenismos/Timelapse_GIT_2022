using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableReparationObjectReceiver : Rewindable
{
    [SerializeField] private ReparationObjectReceiver _repOR = null;
    [System.Serializable]
    struct TimeStampedObjectReceiver
    {
        public float timeStamp;
        public bool isActivated;
        public GameObject held;
        public TimeStampedObjectReceiver(float timeStamp, bool isActivated, GameObject held)
        {
            this.timeStamp = timeStamp;
            this.isActivated = isActivated;
            this.held = held;
        }
    }

    [SerializeField]
    private List<TimeStampedObjectReceiver> ObjectReceiverHistory = new List<TimeStampedObjectReceiver>();

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        //if (ObjectReceiverHistory[0].timeStamp >= totalTime)
        //{
            _repOR.IsActivated = ObjectReceiverHistory[0].isActivated;
            _repOR.held = ObjectReceiverHistory[0].held;
        //}
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
    }

    public override void Record(float timeStamp)
    {
        if (ObjectReceiverHistory.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (ObjectReceiverHistory[0].isActivated != _repOR.IsActivated || ObjectReceiverHistory[0].held != _repOR.held)
            {
                ObjectReceiverHistory.Insert(0, new TimeStampedObjectReceiver(
                timeStamp,
                _repOR.IsActivated,
                _repOR.held
                ));
            }
        }
        else if (ObjectReceiverHistory.Count == 0)
        {
            ObjectReceiverHistory.Insert(0, new TimeStampedObjectReceiver(
            timeStamp,
            _repOR.IsActivated,
            _repOR.held
            ));
        }

        // Reset la liste des timestamp si le temps est inférieur au premier timestamp enregistré (après un rewind très long)
        if (ObjectReceiverHistory.Count == 1)
        {
            if (timeStamp < ObjectReceiverHistory[0].timeStamp)
                ObjectReceiverHistory.Clear();
        }

    }

    private void RewindHistory(float timeStamp)
    {
        if (ObjectReceiverHistory.Count > 1)
        {
            if (ObjectReceiverHistory[0].timeStamp >= timeStamp)
                ObjectReceiverHistory.RemoveAt(0);
        }
    }
}
