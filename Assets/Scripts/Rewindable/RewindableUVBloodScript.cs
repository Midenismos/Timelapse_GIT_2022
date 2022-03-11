using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableUVBloodScript : Rewindable
{
    struct TimeStamped
    {
        public float timeStamp;
        public Color color;
        public float intensity;
        public float fadeCountdown;
        public float fadeLerp;
        public TimeStamped(float timeStamp, Color color, float intensity, float fadeCountdown, float fadeLerp)
        {
            this.timeStamp = timeStamp;
            this.color = color;
            this.intensity = intensity;
            this.fadeCountdown = fadeCountdown;
            this.fadeLerp = fadeLerp;
        }
    }

    private List<TimeStamped> history = new List<TimeStamped>();

    [SerializeField] private UVBloodScript _uvBlood = null;
    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
        if (history.Count != 1)
        {
            if (history[0].timeStamp >= totalTime)
            {
                float t = (1 - (totalTime - history[1].timeStamp)) / (history[0].timeStamp - history[1].timeStamp);
                _uvBlood.Color = history[0].color;
                _uvBlood.FadeCountdown = Mathf.Lerp(history[0].fadeCountdown, history[1].fadeCountdown, t);
                _uvBlood.FadeLerp = Mathf.Lerp(history[0].fadeLerp, history[1].fadeLerp, t);
                _uvBlood.LightIntensity = Mathf.Lerp(history[0].intensity, history[1].intensity, t);
            }
        }
        else
        {
            _uvBlood.Color = history[0].color;
        }
    }

    public override void Record(float timeStamp)
    {
        if (history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (history[0].color != _uvBlood.Color)
            {
                history.Insert(0, new TimeStamped(
                timeStamp,
                _uvBlood.Color,
                _uvBlood.LightIntensity,
                _uvBlood.FadeCountdown,
                _uvBlood.FadeLerp
                )); ;
            }
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStamped(
            timeStamp,
            _uvBlood.Color,
            _uvBlood.LightIntensity,
            _uvBlood.FadeCountdown,
            _uvBlood.FadeLerp
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
