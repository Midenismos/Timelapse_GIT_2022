using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindablePlomb : Rewindable
{
    /*
    [SerializeField] private ReparationPlomb _repPlomb = null;
    [System.Serializable]
    struct TimeStampedObjectReceiver
    {
        public float timeStamp;
        public ObjectReceiver OR;
        public int plombNv;
        public TimeStampedObjectReceiver(float timeStamp, ObjectReceiver OR, int nb)
        {
            this.timeStamp = timeStamp;
            this.OR = OR;
            this.plombNv = nb;
        }
    }

    [SerializeField]
    private List<TimeStampedObjectReceiver> ObjectReceiver = new List<TimeStampedObjectReceiver>();

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
        if (totalTime <= ObjectReceiver[0].timeStamp)
        {
            ObjectReceiver[0].OR.RewindObject();
            _repPlomb.plombNumber = ObjectReceiver[0].plombNv;
            ObjectReceiver.RemoveAt(0);
        }

    }

    public override void Record(float timeStamp)
    {
        if (ObjectReceiver.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (ObjectReceiver[0].currentCode != _repPlomb.CurrentCode)
            {
                ObjectReceiver.Insert(0, new TimeStampedObjectReceiver(
                timeStamp,
                _repPlomb.ob
                ));
            }
        }
        else if (ObjectReceiver.Count == 0)
        {
            ObjectReceiver.Insert(0, new TimeStampedObjectReceiver(
            timeStamp,
            _repPlomb.CurrentCode
            ));
        }

        // Reset la liste des timestamp si le temps est inférieur au premier timestamp enregistré (après un rewind très long)
        if (ObjectReceiver.Count == 1)
        {
            if (timeStamp < ObjectReceiver[0].timeStamp)
                ObjectReceiver.Clear();
        }

    }

    private void RewindHistory(float timeStamp)
    {
        if (ObjectReceiver.Count > 1)
        {
            if (ObjectReceiver[0].timeStamp >= timeStamp)
                ObjectReceiver.RemoveAt(0);
        }
    }
    */
}
