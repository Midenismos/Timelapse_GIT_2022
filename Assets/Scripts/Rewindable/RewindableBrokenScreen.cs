using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableBrokenScreen : Rewindable
{
    private BlinkingBrokenScreen _brokenScreen = null;

    struct TimeStamped
    {
        public float timeStamp;
        public float timerScreenInvisible;
        public float timerScreenVisible;
        public bool screenVisible;

        public TimeStamped(float timeStamp, float timerScreenInvisible, float timerScreenVisible, bool screenVisible)
        {
            this.timeStamp = timeStamp;
            this.timerScreenInvisible = timerScreenInvisible;
            this.timerScreenVisible = timerScreenVisible;
            this.screenVisible = screenVisible;
        }
    }

    private List<TimeStamped> history = new List<TimeStamped>();

    private void Awake()
    {
        _brokenScreen = GetComponent<BlinkingBrokenScreen>();
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
                _brokenScreen.TimerScreenInvisible = Mathf.Lerp(history[0].timerScreenInvisible, history[1].timerScreenInvisible, t);
                _brokenScreen.TimerScreenVisible = Mathf.Lerp(history[0].timerScreenVisible, history[1].timerScreenVisible, t);
                _brokenScreen.ScreenVisible = history[0].screenVisible;

            }
        }
        else
        {
            _brokenScreen.TimerScreenInvisible = history[0].timerScreenInvisible;
            _brokenScreen.TimerScreenVisible = history[0].timerScreenVisible;
            _brokenScreen.ScreenVisible = history[0].screenVisible;
        }
    }

    public override void Record(float timeStamp)
    {
        if (history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (history[0].timerScreenInvisible != _brokenScreen.TimerScreenInvisible || history[0].timerScreenVisible != _brokenScreen.TimerScreenVisible || history[0].screenVisible != _brokenScreen.ScreenVisible)
            {
                history.Insert(0, new TimeStamped(
                timeStamp,
                _brokenScreen.TimerScreenInvisible,
                _brokenScreen.TimerScreenVisible,
                _brokenScreen.ScreenVisible
                )); ;
            }
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStamped(
            timeStamp,
            _brokenScreen.TimerScreenInvisible,
            _brokenScreen.TimerScreenVisible,
            _brokenScreen.ScreenVisible
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
