using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableAlarmLight : Rewindable
{
    private AlarmLight _alarm = null;

    struct TimeStamped
    {
        public float timeStamp;
        public Quaternion rotation;
        public TimeStamped(float timeStamp, Quaternion rotation)
        {
            this.timeStamp = timeStamp;
            this.rotation = rotation;

        }
    }

    private List<TimeStamped> history = new List<TimeStamped>();

    private void Awake()
    {
        _alarm = GetComponent<AlarmLight>();
    }

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
        if (history.Count != 1)
        {
            if (history[0].timeStamp >= totalTime)
            {
                float t = (1 - (totalTime - history[1].timeStamp)) / (history[0].timeStamp - history[1].timeStamp);
                _alarm.LightBulb.transform.rotation = Quaternion.Lerp(history[0].rotation, history[1].rotation, t);
            }
        }
        else
        {
            _alarm.LightBulb.transform.rotation = history[0].rotation;
        }
    }

    public override void Record(float timeStamp)
    {
        if (history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (history[0].rotation != _alarm.LightBulb.transform.rotation)
            {
                history.Insert(0, new TimeStamped(
                timeStamp,
                _alarm.LightBulb.transform.rotation
                )); ;
            }
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStamped(
            timeStamp,
            _alarm.LightBulb.transform.rotation
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
            if (history[0].timeStamp <= timeStamp)
                return;
            else if (history[0].timeStamp >= timeStamp && history[1].timeStamp < timeStamp)
            {
                return;
            }
            history.RemoveAt(0);
        }
    }
}
