using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableMultipleLoreScreen : Rewindable
{
    private MultipleLoreScreen _multipleLoreScreen = null;
    [System.Serializable]
    struct TimeStamped
    {
        public float timeStamp;
        public bool isOnAResult;
        public int infoTxtNumber;

        public TimeStamped(float timeStamp, bool isOnAResult, int infoTxtNumber)
        {
            this.timeStamp = timeStamp;
            this.isOnAResult = isOnAResult;
            this.infoTxtNumber = infoTxtNumber;
        }
    }
    [SerializeField]
    private List<TimeStamped> history = new List<TimeStamped>();

    public override void Start()
    {
        base.Start();
        _multipleLoreScreen = this.GetComponent<MultipleLoreScreen>();
    }

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
        _multipleLoreScreen.IsOnAResult = history[0].isOnAResult;
        _multipleLoreScreen.InfoTxtNumber = history[0].infoTxtNumber;
    }

    public override void Record(float timeStamp)
    {
        if (history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if ( history[0].isOnAResult != _multipleLoreScreen.IsOnAResult || history[0].infoTxtNumber != _multipleLoreScreen.InfoTxtNumber)
                history.Insert(0, new TimeStamped(
                timeStamp,
                _multipleLoreScreen.IsOnAResult,
                _multipleLoreScreen.InfoTxtNumber
                )); ;
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStamped(
            timeStamp,
            _multipleLoreScreen.IsOnAResult,
            _multipleLoreScreen.InfoTxtNumber
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
