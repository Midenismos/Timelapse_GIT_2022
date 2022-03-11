using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableUSB : Rewindable
{
    struct TimeStampedTransform
    {
        public float timeStamp;
        public Vector3 position;
        public Quaternion rotation;
        public Transform parent;

        public TimeStampedTransform(float timeStamp, Vector3 position, Quaternion rotation, Transform parent)
        {
            this.timeStamp = timeStamp;
            this.position = position;
            this.rotation = rotation;
            this.parent = parent;
        }
    }

    [SerializeField]
    private List<TimeStampedTransform> history = new List<TimeStampedTransform>();

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        if (history[0].parent != GameObject.Find("Hand").transform)
            transform.SetParent(history[0].parent, true);
        else
            transform.SetParent(null, true);
        transform.position = history[0].position;
        transform.rotation = history[0].rotation;

        base.Rewind(deltaGameTime, totalTime);

        RewindHistory(totalTime);
    }

    public override void Record(float timeStamp)
    {

        if (history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (history[0].position != transform.position || history[0].rotation != transform.rotation || history[0].parent != transform.parent)
            {
                history.Insert(0, new TimeStampedTransform(
                    timeStamp,
                    transform.position,
                    transform.rotation,
                    transform.parent));
            }
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStampedTransform(
                timeStamp,
                transform.position,
                transform.rotation,
                transform.parent));
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
