using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableWindowLight : Rewindable
{
    [System.Serializable]
    struct TimeStamped
    {
        public float timeStamp;
        public float intensity;
        public float fadeCountdown;
        public float fadeLerp;
        public TimeStamped(float timeStamp, float intensity, float fadeCountdown, float fadeLerp)
        {
            this.timeStamp = timeStamp;
            this.intensity = intensity;
            this.fadeCountdown = fadeCountdown;
            this.fadeLerp = fadeLerp;
        }
    }
    [SerializeField]
    private List<TimeStamped> history = new List<TimeStamped>();

    [SerializeField] private WindowLightScript _windowLight = null;
    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
        if (history.Count != 1)
        {
            if (history[0].timeStamp >= totalTime)
            {
                float t = (1 - (totalTime - history[1].timeStamp)) / (history[0].timeStamp - history[1].timeStamp);
                _windowLight._light.intensity = Mathf.Lerp(history[0].intensity, history[1].intensity, t);
                _windowLight.FadeCountdown = Mathf.Lerp(history[0].fadeCountdown, history[1].fadeCountdown, t);
                _windowLight.FadeLerp = Mathf.Lerp(history[0].fadeLerp, history[1].fadeLerp, t);

            }
        }
        else
        {
            _windowLight._light.intensity = history[0].intensity;
        }
    }

    public override void Record(float timeStamp)
    {
        if (history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (history[0].intensity != _windowLight._light.intensity)
            {
                history.Insert(0, new TimeStamped(
                timeStamp,
                _windowLight._light.intensity,
                _windowLight.FadeCountdown,
                _windowLight.FadeLerp
                )); ;
            }
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStamped(
            timeStamp,
            _windowLight._light.intensity,
            _windowLight.FadeCountdown,
            _windowLight.FadeLerp
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
