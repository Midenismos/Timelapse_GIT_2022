using System;
using System.Collections.Generic;
using UnityEngine;

public class RewindableReactor : Rewindable
{
    private Reactor reac= null;
    [Serializable]
    struct TimeStamped
    {
        public float timeStamp;
        public Vector3 position;
        public int currentWaypoint;

        public TimeStamped(float timeStamp, Vector3 position, int curWayPoint)
        {
            this.timeStamp = timeStamp;
            this.position = position;
            this.currentWaypoint = curWayPoint;
        }
    }
    [SerializeField]
    private List<TimeStamped> history = new List<TimeStamped>();

    public override void Start()
    {
        base.Start();
        reac = this.GetComponent<Reactor>();
    }

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        RewindHistory(totalTime);
        if (history.Count > 1)
        {
            if (history[0].timeStamp >= totalTime)
            { 
                transform.position = history[0].position;
                reac.curWayPointNumber = history[0].currentWaypoint;
            }
        }
        else
        {
            transform.position = history[0].position;
            reac.curWayPointNumber = history[0].currentWaypoint;
        }
    }

    public override void Record(float timeStamp)
    {
        if(history.Count >= 1)
        {
            // NOUVEAU système qui n'enregistre QUE si il y a eu une modification
            if (history[0].position != transform.position || history[0].currentWaypoint != reac.curWayPointNumber)
            {
                history.Insert(0, new TimeStamped(
                timeStamp,
                transform.position,
                reac.curWayPointNumber
                ));
            }
        }
        else if (history.Count == 0)
        {
            history.Insert(0, new TimeStamped(
            timeStamp,
            transform.position,
            reac.curWayPointNumber
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
