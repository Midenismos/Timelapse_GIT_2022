using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableLoadingLoreScreen : Rewindable
{
    private LoadingLoreScreen _loadingScreen = null;
    [System.Serializable]
    struct TimeStamped
    {
        public float timeStamp;
        public bool loaded;
        public bool hasEnteredResearch;
        public bool hasWrongResearch;
        public bool isOnAResult;
        public float timerLoading;
        public string currentResearch;
        public int infoTxtNumber;

        public TimeStamped(float timeStamp, bool loaded, bool hasEnteredResearch, bool hasWrongResearch, bool isOnAResult, float timerLoading, string currentResearch, int infoTxtNumber)
        {
            this.timeStamp = timeStamp;
            this.loaded = loaded;
            this.hasEnteredResearch = hasEnteredResearch;
            this.hasWrongResearch = hasWrongResearch;
            this.isOnAResult = isOnAResult;
            this.timerLoading = timerLoading;
            this.currentResearch = currentResearch;
            this.infoTxtNumber = infoTxtNumber;
        }
    }
    [SerializeField]
    private List<TimeStamped> history = new List<TimeStamped>();

    public override void Start()
    {
        base.Start();
        _loadingScreen = this.GetComponent<LoadingLoreScreen>();
    }

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
        _loadingScreen.Loaded = history[0].loaded;
        _loadingScreen.HasEnteredResearch = history[0].hasEnteredResearch;
        _loadingScreen.HasWrongResearch = history[0].hasWrongResearch;
        _loadingScreen.IsOnAResult = history[0].isOnAResult;
        _loadingScreen.TimerLoading = history[0].timerLoading;
        _loadingScreen.CurrentResearch = history[0].currentResearch;
        _loadingScreen.InfoTxtNumber = history[0].infoTxtNumber;
    }

    public override void Record(float timeStamp)
    {
        if (history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (history[0].loaded != _loadingScreen.Loaded || history[0].hasEnteredResearch != _loadingScreen.HasEnteredResearch || history[0].hasWrongResearch != _loadingScreen.HasWrongResearch || history[0].isOnAResult != _loadingScreen.IsOnAResult || history[0].timerLoading != _loadingScreen.TimerLoading || history[0].currentResearch != _loadingScreen.CurrentResearch || history[0].infoTxtNumber != _loadingScreen.InfoTxtNumber)
                history.Insert(0, new TimeStamped(
                timeStamp,
                _loadingScreen.Loaded,
                _loadingScreen.HasEnteredResearch,
                _loadingScreen.HasWrongResearch,
                _loadingScreen.IsOnAResult,
                _loadingScreen.TimerLoading,
                _loadingScreen.CurrentResearch,
                _loadingScreen.InfoTxtNumber
                ));;
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStamped(
            timeStamp,
            _loadingScreen.Loaded,
            _loadingScreen.HasEnteredResearch,
            _loadingScreen.HasWrongResearch,
            _loadingScreen.IsOnAResult,
            _loadingScreen.TimerLoading,
            _loadingScreen.CurrentResearch,
            _loadingScreen.InfoTxtNumber
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
