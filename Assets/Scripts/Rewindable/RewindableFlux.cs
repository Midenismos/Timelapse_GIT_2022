using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableFlux : Rewindable
{
    [SerializeField] private ReparationFluxScript _repFlux = null;
    [System.Serializable]
    struct TimeStamped
    {
        public float timeStamp;
        public int currentButtonNumber;
        public int currentFluxNumber;

        public TimeStamped(float timeStamp, int currentButtonNumber, int currentFluxNumber)
        {
            this.timeStamp = timeStamp;
            this.currentButtonNumber = currentButtonNumber;
            this.currentFluxNumber = currentFluxNumber;
        }
    }
    [SerializeField]
    private List<TimeStamped> history = new List<TimeStamped>();

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
        _repFlux.CurrentButtonNumber = history[0].currentButtonNumber;
        _repFlux.CurrentFluxNumber = history[0].currentFluxNumber;
    }

    public override void Record(float timeStamp)
    {
        if (history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (history[0].currentButtonNumber != _repFlux.CurrentButtonNumber || history[0].currentFluxNumber != _repFlux.CurrentFluxNumber)
            {
                history.Insert(0, new TimeStamped(
                timeStamp,
                _repFlux.CurrentButtonNumber,
                _repFlux.CurrentFluxNumber
                ));
            }
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStamped(
            timeStamp,
            _repFlux.CurrentButtonNumber,
            _repFlux.CurrentFluxNumber
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
    }

}
