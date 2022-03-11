using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindablePasswordScreen : Rewindable
{
    private PasswordScript _passwordScreen = null;
    [System.Serializable]
    struct TimeStamped
    {
        public float timeStamp;
        public bool hasWrongPassword;
        public bool hasCorrectPassword;
        public string currentPassword;

        public TimeStamped(float timeStamp, bool hasWrongPassword, bool hasCorrectPassword, string currentPassword)
        {
            this.timeStamp = timeStamp;
            this.hasWrongPassword = hasWrongPassword;
            this.hasCorrectPassword = hasCorrectPassword;
            this.currentPassword = currentPassword;
        }
    }
    [SerializeField]
    private List<TimeStamped> history = new List<TimeStamped>();

    public override void Start()
    {
        base.Start();
        _passwordScreen = this.GetComponent<PasswordScript>();
    }

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
        _passwordScreen.HasCorrectPassword = history[0].hasCorrectPassword;
        _passwordScreen.HasWrongPassword = history[0].hasWrongPassword;
        _passwordScreen.CurrentPassword = history[0].currentPassword;
    }

    public override void Record(float timeStamp)
    {
        if (history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (history[0].hasWrongPassword != _passwordScreen.HasWrongPassword || history[0].hasCorrectPassword != _passwordScreen.HasCorrectPassword || history[0].currentPassword != _passwordScreen.CurrentPassword)
                history.Insert(0, new TimeStamped(
                timeStamp,
                _passwordScreen.HasWrongPassword,
                _passwordScreen.HasCorrectPassword,
                _passwordScreen.CurrentPassword
                ));
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStamped(
            timeStamp,
            _passwordScreen.HasWrongPassword,
            _passwordScreen.HasCorrectPassword,
            _passwordScreen.CurrentPassword
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
