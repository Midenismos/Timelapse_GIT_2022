using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableCalibrage : Rewindable
{
    [SerializeField] private ReparationCalibrageScript _repCalibrage = null;
    [System.Serializable]
    struct TimeStamped
    {
        public float timeStamp;
        public string currentCode;

        public TimeStamped(float timeStamp, string currentCode)
        {
            this.timeStamp = timeStamp;
            this.currentCode = currentCode;
        }
    }
    [SerializeField]
    private List<TimeStamped> history = new List<TimeStamped>();

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
        _repCalibrage.CurrentCode = history[0].currentCode;
    }

    public override void Record(float timeStamp)
    {
        if (history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (history[0].currentCode != _repCalibrage.CurrentCode)
            {
                history.Insert(0, new TimeStamped(
                timeStamp,
                _repCalibrage.CurrentCode
                ));
            }
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStamped(
            timeStamp,
            _repCalibrage.CurrentCode
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
